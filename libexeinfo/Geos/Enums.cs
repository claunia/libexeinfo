//
// Enums.cs
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
// THE SOFTWARE.namespace libexeinfo.Geos

using System;

namespace libexeinfo
{
    public partial class Geos
    {
        [Flags]
        enum Attributes : ushort
        {
            GA_PROCESS                   = 0x8000,
            GA_LIBRARY                   = 0x4000,
            GA_DRIVER                    = 0x2000,
            GA_KEEP_FILE_OPEN            = 0x1000,
            GA_SYSTEM                    = 0x0800,
            GA_MULTI_LAUNCHABLE          = 0x0400,
            GA_APPLICATION               = 0x0200,
            GA_DRIVER_INITIALIZED        = 0x0100,
            GA_LIBRARY_INITIALIZED       = 0x0080,
            GA_GEODE_INITIALIZED         = 0x0040,
            GA_USES_COPROC               = 0x0020,
            GA_REQUIRES_COPROC           = 0x0010,
            GA_HAS_GENERAL_CONSUMER_MODE = 0x0008,
            GA_ENTRY_POINTS_IN_C         = 0x0004
        }

        enum FileType2 : ushort
        {
            /// <summary>
            ///     The file is not a GEOS file.
            /// </summary>
            GFT_NOT_GEOS_FILE = 0,
            /// <summary>
            ///     The file is executable.
            /// </summary>
            GFT_EXECUTABLE = 1,
            /// <summary>
            ///     The file is a VM file.
            /// </summary>
            GFT_VM = 2,
            /// <summary>
            ///     The file is a GEOS byte file.
            /// </summary>
            GFT_DATA = 3,
            /// <summary>
            ///     The file is a GEOS directory.
            /// </summary>
            GFT_DIRECTORY = 4,
            /// <summary>
            ///     The file is a symbolic link.
            /// </summary>
            GFT_LINK = 5
        }

        enum ApplicationType : ushort
        {
            Application = 1,
            Library     = 2,
            Driver      = 3
        }

        enum FileType : ushort
        {
            /// <summary>
            ///     The file is executable.
            /// </summary>
            GFT_EXECUTABLE = 0,
            /// <summary>
            ///     The file is a VM file.
            /// </summary>
            GFT_VM = 1
        }

        // These are defined in GEOS SDK but itself says they stopped using numerical IDs and should use ASCII ones.
        enum ManufacturerId : ushort
        {
            GeoWorks      = 0,
            App           = 1,
            Palm          = 2,
            Wizard        = 3,
            CreativeLabs  = 4,
            DosLauncher   = 5,
            AmericaOnline = 6,
            Intuit        = 7,
            Sdk           = 8,
            Agd           = 9,
            Generic       = 10,
            Tbd           = 11,
            Socket        = 12
        }

        [Flags]
        enum SegmentFlags : ushort
        {
            /// <summary>
            ///     The block will not move from its place in the global heap until it is freed.
            /// </summary>
            HF_FIXED = 0x80,
            /// <summary>
            ///     The block may be locked by threads belonging to geodes other than the block's owner.
            /// </summary>
            HF_SHARABLE = 0x40,
            /// <summary>
            ///     The block may be discarded when unlocked.
            /// </summary>
            HF_DISCARDABLE = 0x20,
            /// <summary>
            ///     The block may be swapped to extended/expanded memory or to the disk swap space when it is unlocked.
            /// </summary>
            HF_SWAPABLE = 0x10,
            /// <summary>
            ///     The block contains a local memory heap.
            /// </summary>
            HF_LMEM = 0x08,
            /// <summary>
            ///     The memory manager turns this bit on when it discards a block.
            /// </summary>
            HF_DISCARDED = 0x02,
            /// <summary>
            ///     The memory manager turns this bit on when it swaps a block to extended/expanded memory or to the disk swap space.
            /// </summary>
            HF_SWAPPED = 0x01,
            HF_STATIC  = HF_DISCARDABLE | HF_SWAPABLE,
            HF_DYNAMIC = HF_SWAPABLE,
            /// <summary>
            ///     The memory manager should initialize the block to null bytes.
            /// </summary>
            HAF_ZERO_INIT = 0x8000,
            /// <summary>
            ///     The memory manager should lock the block after allocating it.
            /// </summary>
            HAF_LOCK = 0x4000,
            /// <summary>
            ///     The memory manager should not return errors. If it cannot allocate block, GEOS will tell the user that there is no
            ///     memory available and crash.
            /// </summary>
            HAF_NO_ERR = 0x2000,
            /// <summary>
            ///     If both <see cref="HAF_UI" /> and <see cref="HAF_OBJECT_RESOURCE" /> are set, this block will be run by the
            ///     application's UI thread.
            /// </summary>
            HAF_UI = 0x1000,
            /// <summary>
            ///     The block's data will not be modified.
            /// </summary>
            HAF_READ_ONLY = 0x0800,
            /// <summary>
            ///     This block will be an object block.
            /// </summary>
            HAF_OBJECT_RESOURCE = 0x0400,
            /// <summary>
            ///     This block contains executable code.
            /// </summary>
            HAF_CODE = 0x0200,
            /// <summary>
            ///     If the block contains code, the code may be run by a less privileged entity. If the block contains data, the data
            ///     may be accessed or altered by a less privileged entity.
            /// </summary>
            HAF_CONFORMING = 0x0100
        }
    }
}