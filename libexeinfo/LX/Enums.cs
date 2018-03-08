//
// Enums.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017-2018 Copyright © Claunia.com
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

namespace libexeinfo
{
    public partial class LX
    {
        /// <summary>
        ///     Executable module flags.
        /// </summary>
        [Flags]
        public enum ModuleFlags : uint
        {
            /// <summary>
            ///     Per-Process Library Initialization
            /// </summary>
            PerProcessLibrary = 0x04,
            /// <summary>
            ///     Internal fixups for the module have been applied
            /// </summary>
            InternalFixups = 0x10,
            /// <summary>
            ///     External fixups for the module have been applied
            /// </summary>
            ExternalFixups = 0x20,
            /// <summary>
            ///     Incompatible with Presentation Manager
            /// </summary>
            PMIncompatible = 0x100,
            /// <summary>
            ///     Compatible with Presentation Manager
            /// </summary>
            PMCompatible = 0x200,
            /// <summary>
            ///     Uses Presentation Manager
            /// </summary>
            UsesPM = 0x300,
            /// <summary>
            ///     Module is not loadable. Contains errors or is being linked
            /// </summary>
            NotLoadable = 0x2000,
            /// <summary>
            ///     Library module
            /// </summary>
            Library = 0x8000,
            /// <summary>
            ///     Protected Memory Library module
            /// </summary>
            ProtectedMemoryLibrary = 0x18000,
            /// <summary>
            ///     Physical Device Driver module
            /// </summary>
            PhysicalDeviceDriver = 0x20000,
            /// <summary>
            ///     Virtual Device Driver module
            /// </summary>
            VirtualDeviceDriver = 0x28000,
            /// <summary>
            ///     Per-process Library Termination
            /// </summary>
            PerProcessTermination = 0x40000000
        }

        public enum TargetCpu : ushort
        {
            Unknown = 0,
            i286    = 1,
            i386    = 2,
            i486    = 3,
            Pentium = 4,
            i860    = 0x20,
            N11     = 0x21,
            MIPS1   = 0x40,
            MIPS2   = 0x41,
            MIPS3   = 0x42
        }

        /// <summary>
        ///     Target operating system.
        /// </summary>
        public enum TargetOS : ushort
        {
            Unknown = 0,
            OS2     = 1,
            Windows = 2,
            DOS     = 3,
            Win32   = 4,
            NT      = 0x20,
            Posix   = 0x21
        }

        static Architecture CpuToArchitecture(TargetCpu cpu)
        {
            switch(cpu)
            {
                case TargetCpu.i286: return Architecture.I286;
                case TargetCpu.i386:
                case TargetCpu.i486:
                case TargetCpu.Pentium: return Architecture.I386;
                case TargetCpu.i860:
                case TargetCpu.N11: return Architecture.I860;
                case TargetCpu.MIPS1:
                case TargetCpu.MIPS2: return Architecture.Mips;
                case TargetCpu.MIPS3: return Architecture.Mips3;
                default:              return Architecture.Unknown;
            }
        }

        [Flags]
        enum ObjectFlags
        {
            Readable             = 0x0001,
            Writable             = 0x0002,
            Executable           = 0x0004,
            Resource             = 0x0008,
            Discardable          = 0x0010,
            Shared               = 0x0020,
            Preload              = 0x0040,
            Invalid              = 0x0080,
            Zeroed               = 0x0100,
            Resident             = 0x0200,
            Contiguous           = 0x0300,
            LongLockable         = 0x0400,
            Reserved             = 0x0800,
            Alias1616Required    = 0x1000,
            BigDefaultBitSetting = 0x2000,
            Conforming           = 0x4000,
            Privilege            = 0x8000
        }

        enum PageTableAttributes : ushort
        {
            /// <summary>
            ///     Offset from preload page section
            /// </summary>
            LegalPhysicalPage = 0,
            /// <summary>
            ///     Offset from iterated page section
            /// </summary>
            IteratedDataPage = 1,
            Invalid      = 2,
            Zeroed       = 3,
            RangeOfPages = 4
        }

        enum PageTableAttributes16 : byte
        {
            /// <summary>
            ///     Offset from preload page section
            /// </summary>
            LegalPhysicalPage = (byte)PageTableAttributes.LegalPhysicalPage,
            /// <summary>
            ///     Offset from iterated page section
            /// </summary>
            IteratedDataPage = (byte)PageTableAttributes.IteratedDataPage,
            Invalid      = (byte)PageTableAttributes.Invalid,
            Zeroed       = (byte)PageTableAttributes.Zeroed,
            RangeOfPages = (byte)PageTableAttributes.RangeOfPages
        }
    }
}