//
// NE.cs
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
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace libexeinfo
{
    /// <summary>
    ///     Represents a Microsoft New Executable
    /// </summary>
    public partial class NE : IExecutable
    {
        MZ BaseExecutable;
        /// <summary>
        ///     Header for this executable
        /// </summary>
        public NEHeader      Header;
        public ResourceTable Resources;
        public Version[]     Versions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="path">Executable path.</param>
        public NE(string path)
        {
            BaseStream = File.Open(path, FileMode.Open, FileAccess.Read);
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="stream">Stream containing the executable.</param>
        public NE(Stream stream)
        {
            BaseStream = stream;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:libexeinfo.NE" /> class.
        /// </summary>
        /// <param name="data">Stream containing the executable.</param>
        public NE(byte[] data)
        {
            BaseStream = new MemoryStream(data);
            Initialize();
        }

        public Stream                    BaseStream    { get; }
        public bool                      IsBigEndian   => false;
        public bool                      Recognized    { get; private set; }
        public string                    Type          { get; private set; }
        public IEnumerable<Architecture> Architectures =>
            new[]
            {
                Header.target_os == TargetOS.Win32 || Header.program_flags.HasFlag(ProgramFlags.i386)
                    ? Architecture.I386
                    : Header.target_os == TargetOS.OS2 || Header.program_flags.HasFlag(ProgramFlags.i286)
                        ? Architecture.I286
                        : Architecture.I86
            };
        public OperatingSystem RequiredOperatingSystem { get; private set; }

        void Initialize()
        {
            Recognized = false;

            if(BaseStream == null) return;

            BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            if(Header.signature != SIGNATURE) return;

            Recognized = true;
            Type       = "New Executable (NE)";

            OperatingSystem reqOs = new OperatingSystem();

            switch(Header.target_os)
            {
                case TargetOS.OS2:
                    reqOs.Name = "OS/2";
                    if(Header.os_major > 0)
                    {
                        reqOs.MajorVersion = Header.os_major;
                        reqOs.MinorVersion = Header.os_minor;
                    }
                    else
                    {
                        reqOs.MajorVersion = 1;
                        reqOs.MinorVersion = 0;
                    }

                    if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                       !Header.application_flags.HasFlag(ApplicationFlags.GUICompatible) ||
                       !Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                       Header.application_flags.HasFlag(ApplicationFlags.GUICompatible)) reqOs.Subsystem = "Console";
                    else if(Header.application_flags.HasFlag(ApplicationFlags.FullScreen) &&
                            Header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
                        reqOs.Subsystem = "Presentation Manager";
                    break;
                case TargetOS.Windows:
                case TargetOS.Win32:
                case TargetOS.Unknown:
                    reqOs.Name = "Windows";
                    if(Header.os_major > 0)
                    {
                        reqOs.MajorVersion = Header.os_major;
                        reqOs.MinorVersion = Header.os_minor;
                    }
                    else
                        switch(Header.target_os)
                        {
                            case TargetOS.Windows:
                                reqOs.MajorVersion = 2;
                                reqOs.MinorVersion = 0;
                                break;
                            case TargetOS.Unknown:
                                reqOs.MajorVersion = 1;
                                reqOs.MinorVersion = 0;
                                break;
                        }

                    break;
                case TargetOS.DOS:
                case TargetOS.Borland:
                    reqOs.Name                                               = "DOS";
                    reqOs.MajorVersion                                       = Header.os_major;
                    reqOs.MinorVersion                                       = Header.os_minor;
                    if(Header.target_os == TargetOS.Borland) reqOs.Subsystem = "Borland Operating System Services";
                    break;
                default:
                    reqOs.Name         = $"Unknown code {(byte)Header.target_os}";
                    reqOs.MajorVersion = Header.os_major;
                    reqOs.MinorVersion = Header.os_minor;
                    break;
            }

            RequiredOperatingSystem = reqOs;

            if(Header.resource_entries <= 0) return;

            Resources = GetResources(BaseStream, BaseExecutable.Header.new_offset, Header.resource_table_offset);
            Versions  = GetVersions().ToArray();
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft New Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft New Executable, <c>false</c> otherwise.</returns>
        /// <param name="path">Executable path.</param>
        public static bool Identify(string path)
        {
            FileStream BaseStream     = File.Open(path, FileMode.Open, FileAccess.Read);
            MZ         BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return false;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return false;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            NEHeader Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return Header.signature == SIGNATURE;
        }

        /// <summary>
        ///     Identifies if the specified executable is a Microsoft New Executable
        /// </summary>
        /// <returns><c>true</c> if the specified executable is a Microsoft New Executable, <c>false</c> otherwise.</returns>
        /// <param name="stream">Stream containing the executable.</param>
        public static bool Identify(FileStream stream)
        {
            FileStream BaseStream     = stream;
            MZ         BaseExecutable = new MZ(BaseStream);
            if(!BaseExecutable.Recognized) return false;

            if(BaseExecutable.Header.new_offset >= BaseStream.Length) return false;

            BaseStream.Seek(BaseExecutable.Header.new_offset, SeekOrigin.Begin);
            byte[] buffer = new byte[Marshal.SizeOf(typeof(NEHeader))];
            BaseStream.Read(buffer, 0, buffer.Length);
            IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
            NEHeader Header = (NEHeader)Marshal.PtrToStructure(hdrPtr, typeof(NEHeader));
            Marshal.FreeHGlobal(hdrPtr);
            return Header.signature == SIGNATURE;
        }
    }
}