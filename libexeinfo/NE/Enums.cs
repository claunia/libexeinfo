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
    public partial class NE
    {
        /// <summary>
        ///     Application flags.
        /// </summary>
        [Flags]
        public enum ApplicationFlags : byte
        {
            FullScreen    = 1,
            GUICompatible = 2,
            Errors        = 1 << 5,
            NonConforming = 1 << 6,
            DLL           = 1 << 7
        }

        /// <summary>
        ///     Program flags.
        /// </summary>
        [Flags]
        public enum ProgramFlags : byte
        {
            NoDGroup       = 0,
            SingleDGroup   = 1,
            MultipleDGroup = 2,
            GlobalInit     = 1 << 2,
            ProtectedMode  = 1 << 3,
            i86            = 1 << 4,
            i286           = 1 << 5,
            i386           = 1 << 6,
            i87            = 1 << 7
        }

        /// <summary>
        ///     Resource flags.
        /// </summary>
        [Flags]
        public enum ResourceFlags : ushort
        {
            Moveable       = 0x10,
            Pure           = 0x20,
            Preload        = 0x40,
            Discardable    = 0x1000,
            SegmentAligned = 0x8000
        }

        [Flags]
        public enum SegmentFlags : ushort
        {
            /// <summary>
            ///     Segment data is iterated
            /// </summary>
            Iterated = 0x08,
            /// <summary>
            ///     Segment is not fixed
            /// </summary>
            Moveable = 0x10,
            /// <summary>
            ///     Segment can be shared
            /// </summary>
            Shared = 0x20,
            /// <summary>
            ///     Segment will be preloaded; read-only if this is a data segment
            /// </summary>
            Preload = 0x40,
            /// <summary>
            ///     Code segment is execute only; data segment is read-only
            /// </summary>
            Eronly = 0x80,
            /// <summary>
            ///     Segment has relocation records
            /// </summary>
            Relocinfo = 0x100,
            /// <summary>
            ///     Segment is conforming
            /// </summary>
            Conform = 0x200,
            /// <summary>
            ///     Discardable
            /// </summary>
            Discardable = 0x1000,
            /// <summary>
            ///     32-bit code segment
            /// </summary>
            Code32 = 0x2000,
            /// <summary>
            ///     Length of segment and minimum allocation size are in units of segment sector size
            /// </summary>
            Huge = 0x4000
        }

        public enum SegmentType : ushort
        {
            Code = 0,
            Data = 1
        }

        /// <summary>
        ///     Target operating system.
        /// </summary>
        public enum TargetOS : byte
        {
            Unknown = 0,
            OS2     = 1,
            Windows = 2,
            DOS     = 3,
            Win32   = 4,
            Borland = 5
        }
    }
}