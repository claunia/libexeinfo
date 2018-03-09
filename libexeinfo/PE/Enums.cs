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

// ReSharper disable InconsistentNaming

namespace libexeinfo
{
    public partial class PE
    {
        enum DebugTypes : uint
        {
            /// <summary>
            ///     Unknown value, ignored by all tools.
            /// </summary>
            IMAGE_DEBUG_TYPE_UNKNOWN = 0,
            /// <summary>
            ///     COFF debug information (line numbers,  symbol table, and string table).
            ///     This type of debug information is also pointed to by fields in the file headers.
            /// </summary>
            IMAGE_DEBUG_TYPE_COFF = 1,
            /// <summary>
            ///     CodeView debug information. The format of the data block is described
            ///     by the CV4 specification.
            /// </summary>
            IMAGE_DEBUG_TYPE_CODEVIEW = 2,
            /// <summary>
            ///     Frame Pointer Omission (FPO) information. This information tells the
            ///     debugger how to interpret non-standard stack frames, which use the
            ///     EBP register for a purpose other than as a frame pointer.
            /// </summary>
            IMAGE_DEBUG_TYPE_FPO = 3,
            IMAGE_DEBUG_TYPE_MISC          = 4,
            IMAGE_DEBUG_TYPE_EXCEPTION     = 5,
            IMAGE_DEBUG_TYPE_FIXUP         = 6,
            IMAGE_DEBUG_TYPE_OMAP_TO_SRC   = 7,
            IMAGE_DEBUG_TYPE_OMAP_FROM_SRC = 8,
            IMAGE_DEBUG_TYPE_BORLAND       = 9
        }

        /// <summary>
        ///     The following values are defined for the DllCharacteristics field of the optional header.
        /// </summary>
        [Flags]
        enum DllCharacteristics : ushort
        {
            /// <summary>
            ///     Image can handle a high entropy 64-bit virtual address space.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_HIGH_ENTROPY_VA = 0x0020,
            /// <summary>
            ///     DLL can be relocated at load time.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE = 0x0040,
            /// <summary>
            ///     Code Integrity checks are enforced.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_FORCE_INTEGRITY = 0x0080,
            /// <summary>
            ///     Image is NX compatible.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NX_COMPAT = 0x0100,
            /// <summary>
            ///     Isolation aware, but do not isolate the image.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200,
            /// <summary>
            ///     Does not use structured exception (SE) handling. No SE handler may be called in this image.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400,
            /// <summary>
            ///     Do not bind the image.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800,
            /// <summary>
            ///     Image must execute in an AppContainer.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_APPCONTAINER = 0x1000,
            /// <summary>
            ///     A WDM driver.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
            /// <summary>
            ///     Image supports Control Flow Guard.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_GUARD_CF = 0x4000,
            /// <summary>
            ///     Terminal Server aware.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
        }

        /// <summary>
        ///     The following values defined for the Subsystem field of the optional header determine which Windows subsystem (if
        ///     any) is required to run the image.
        /// </summary>
        enum Subsystems : ushort
        {
            /// <summary>
            ///     An unknown subsystem
            /// </summary>
            IMAGE_SUBSYSTEM_UNKNOWN = 0,
            /// <summary>
            ///     Device drivers and native Windows processes
            /// </summary>
            IMAGE_SUBSYSTEM_NATIVE = 1,
            /// <summary>
            ///     The Windows graphical user interface (GUI) subsystem
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,
            /// <summary>
            ///     The Windows character subsystem
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,
            /// <summary>
            ///     The OS/2 character subsystem
            /// </summary>
            IMAGE_SUBSYSTEM_OS2_CUI = 5,
            /// <summary>
            ///     The Posix character subsystem
            /// </summary>
            IMAGE_SUBSYSTEM_POSIX_CUI = 7,
            /// <summary>
            ///     Native Win9x driver
            /// </summary>
            IMAGE_SUBSYSTEM_NATIVE_WINDOWS = 8,
            /// <summary>
            ///     Windows CE
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,
            /// <summary>
            ///     An Extensible Firmware Interface (EFI) application
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,
            /// <summary>
            ///     An EFI driver with boot services
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,
            /// <summary>
            ///     An EFI driver with run-time services
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,
            /// <summary>
            ///     An EFI ROM image
            /// </summary>
            IMAGE_SUBSYSTEM_EFI_ROM = 13,
            /// <summary>
            ///     XBOX
            /// </summary>
            IMAGE_SUBSYSTEM_XBOX = 14,
            /// <summary>
            ///     Windows boot application
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION = 16
        }
    }
}