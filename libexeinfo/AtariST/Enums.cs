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
    public partial class AtariST
    {
        [Flags]
        public enum PrgFlags : ushort
        {
            /// <summary>
            ///     If set clear only the BSS area on program load, otherwise clear the entire heap.
            /// </summary>
            PF_FASTLOAD = 0x01,
            /// <summary>
            ///     If set, the program may be loaded into alternative RAM, otherwise it muse be loaded into standard RAM.
            /// </summary>
            PF_TTRAMLOAD = 0x02,
            /// <summary>
            ///     If set, the program's malloc() requests may be satisfied from alternative RAM, otherwise they must be satisfied
            ///     from standard RAM.
            /// </summary>
            PF_TTRAMMEM = 0x04
        }

        public enum PrgSharing : ushort
        {
            /// <summary>
            ///     The processes' entire memory space will be considered private.
            /// </summary>
            PF_PRIVATE = 0,
            /// <summary>
            ///     The processes' entire memory space will be readable and writable by any process.
            /// </summary>
            PF_GLOBAL = 1,
            /// <summary>
            ///     The processes' entire memory space will only be readable and writable by itself and any other process in supervisor
            ///     mode.
            /// </summary>
            PF_SUPERVISOR = 2,
            /// <summary>
            ///     The processes' entire memory space will be readable by any application but only writable by itself.
            /// </summary>
            PF_READABLE = 3
        }

        [Flags]
        public enum SymbolType : ushort
        {
            Bss             = 0x0100,
            Text            = 0x0200,
            Data            = 0x0400,
            External        = 0x0800,
            EquatedRegister = 0x1000,
            Global          = 0x2000,
            Equated         = 0x4000,
            Defined         = 0x8000
        }
    }
}