//
// PE.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017 Copyright © Claunia.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITPESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONPECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace libexeinfo
{
    /// <summary>
    ///     Represents a Microsoft Portable Executable
    /// </summary>
    public partial class PE : IExecutable
    {
        MZ BaseExecutable;
        /// <summary>
        ///     Header for this executable
        /// </summary>
        PEHeader        Header;
        WindowsHeader64 WinHeader;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public PE(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public PE(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.PE" /> class.
        /// </summary>
        /// <param name="data">Byte array containing the executable.</param>
        public PE(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        public Stream                    BaseStream    { get; }
        public bool                      IsBigEndian   => false;
        public bool                      Recognized    { get; private set; }
        public string                    Type          { get; private set; }
        public IEnumerable<Architecture> Architectures =>
            new[] {COFF.MachineTypeToArchitecture(Header.coff.machine)};
        public OperatingSystem RequiredOperatingSystem { get; private set; }
        public IEnumerable<string> Strings { get; }
        public IEnumerable<Segment> Segments { get; }

        void Initialize()
        {
            Recognized = false;
            if(BaseStream == null) return;

            BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            Recognized = Header.signature == SIGNATURE;

            if(!Recognized) return;

            Type = "Portable Executable (PE)";

            if(Header.coff.optionalHeader.magic == PE32Plus)
            {
                BaseStream.Position -= 4;
                buffer              =  new byte[Marshal.SizeOf(typeof(WindowsHeader64))];
                BaseStream.Read(buffer, 0, buffer.Length);
                hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                WinHeader = (WindowsHeader64)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader64));
                Marshal.FreeHGlobal(hdrPtr);
            }
            else
            {
                buffer = new byte[Marshal.SizeOf(typeof(WindowsHeader))];
                BaseStream.Read(buffer, 0, buffer.Length);
                hdrPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
                WindowsHeader hdr32 = (WindowsHeader)Marshal.PtrToStructure(hdrPtr, typeof(WindowsHeader));
                Marshal.FreeHGlobal(hdrPtr);
                WinHeader = ToPlus(hdr32);
            }

            OperatingSystem reqOs = new OperatingSystem();

            switch(WinHeader.subsystem)
            {
                case Subsystems.IMAGE_SUBSYSTEM_UNKNOWN:
                    reqOs.Name = "Unknown";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_NATIVE:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "Native";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_GUI:
                    reqOs.Name      = WinHeader.majorOperatingSystemVersion < 3 ? "Windows NT" : "Windows";
                    reqOs.Subsystem = "GUI";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_CUI:
                    reqOs.Name      = WinHeader.majorOperatingSystemVersion < 3 ? "Windows NT" : "Windows";
                    reqOs.Subsystem = "Console";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_OS2_CUI:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "OS/2";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_POSIX_CUI:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "POSIX";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_NATIVE_WINDOWS:
                    reqOs.Name      = "Windows";
                    reqOs.Subsystem = "Native";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_CE_GUI:
                    reqOs.Name = "Windows CE";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_EFI_APPLICATION:
                case Subsystems.IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER:
                case Subsystems.IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER:
                case Subsystems.IMAGE_SUBSYSTEM_EFI_ROM:
                    reqOs.Name = "EFI";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_XBOX:
                    reqOs.Name = "Xbox OS";
                    break;
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION:
                    reqOs.Name      = "Windows NT";
                    reqOs.Subsystem = "Boot environment";
                    break;
                default:
                    reqOs.Name = $"Unknown code ${(ushort)WinHeader.subsystem}";
                    break;
            }

            reqOs.MajorVersion      = WinHeader.majorOperatingSystemVersion;
            reqOs.MinorVersion      = WinHeader.minorOperatingSystemVersion;
            RequiredOperatingSystem = reqOs;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft Portable Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft Portable Executable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream baseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         baseExecutable = new MZ(baseStream);
            if(!baseExecutable.Recognized) return false;

            if(baseExecutable.Header.new_offset >= baseStream.Length) return false;

            baseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
            baseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            PEHeader header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return header.signature == SIGNATURE;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft Portable Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft Portable Executable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            FileStream baseStream     = stream;
            MZ         baseExecutable = new MZ(baseStream);
            if(!baseExecutable.Recognized) return false;

            if(baseExecutable.Header.new_offset >= baseStream.Length) return false;

            baseStream.Seek(baseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(PEHeader))];
            baseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            PEHeader header = (PEHeader)Marshal.PtrToStructure(hdrPtr, typeof(PEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return header.signature == SIGNATURE;
        }

        static WindowsHeader64 ToPlus(WindowsHeader header)
        {
            return new WindowsHeader64
            {
                imageBase                   = header.imageBase,
                sectionAlignment            = header.sectionAlignment,
                fileAlignment               = header.fileAlignment,
                majorOperatingSystemVersion = header.majorOperatingSystemVersion,
                minorOperatingSystemVersion = header.minorOperatingSystemVersion,
                majorImageVersion           = header.majorImageVersion,
                minorImageVersion           = header.minorImageVersion,
                majorSubsystemVersion       = header.majorSubsystemVersion,
                minorSubsystemVersion       = header.minorSubsystemVersion,
                win32VersionValue           = header.win32VersionValue,
                sizeOfImage                 = header.sizeOfImage,
                sizeOfHeaders               = header.sizeOfHeaders,
                checksum                    = header.checksum,
                subsystem                   = header.subsystem,
                dllCharacteristics          = header.dllCharacteristics,
                sizeOfStackReserve          = header.sizeOfStackReserve,
                sizeOfStackCommit           = header.sizeOfStackCommit,
                sizeOfHeapReserve           = header.sizeOfHeapReserve,
                sizeOfHeapCommit            = header.sizeOfHeapCommit,
                loaderFlags                 = header.loaderFlags,
                numberOfRvaAndSizes         = header.numberOfRvaAndSizes
            };
        }
    }
}