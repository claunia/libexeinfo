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
        public enum DebugTypes : uint
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
            IMAGE_DEBUG_TYPE_FPO           = 3,
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
        public enum DllCharacteristics : ushort
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
        public enum Subsystems : ushort
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
        
         /// <summary>
        ///     Version file flags.
        /// </summary>
        [Flags]
        public enum VersionFileFlags : uint
        {
            VS_FF_DEBUG        = 0x00000001,
            VS_FF_INFOINFERRED = 0x00000010,
            VS_FF_PATCHED      = 0x00000004,
            VS_FF_PRERELEASE   = 0x00000002,
            VS_FF_PRIVATEBUILD = 0x00000008,
            VS_FF_SPECIALBUILD = 0x00000020
        }

        /// <summary>
        ///     Version file operating system.
        /// </summary>
        public enum VersionFileOS : uint
        {
            VOS_DOS       = 0x00010000,
            VOS_NT        = 0x00040000,
            VOS_WINDOWS16 = 0x00000001,
            VOS_WINDOWS32 = 0x00000004,
            VOS_OS216     = 0x00020000,
            VOS_OS232     = 0x00030000,
            VOS_PM16      = 0x00000002,
            VOS_PM32      = 0x00000003,
            VOS_UNKNOWN   = 0x00000000,

            // Combinations, some have no sense
            VOS_DOS_NT          = 0x00050000,
            VOS_DOS_WINDOWS16   = 0x00010001,
            VOS_DOS_WINDOWS32   = 0x00010004,
            VOS_DOS_PM16        = 0x00010002,
            VOS_DOS_PM32        = 0x00010003,
            VOS_NT_WINDOWS16    = 0x00040001,
            VOS_NT_WINDOWS32    = 0x00040004,
            VOS_NT_PM16         = 0x00040002,
            VOS_NT_PM32         = 0x00040003,
            VOS_OS216_WINDOWS16 = 0x00020001,
            VOS_OS216_WINDOWS32 = 0x00020004,
            VOS_OS216_PM16      = 0x00020002,
            VOS_OS216_PM32      = 0x00020003,
            VOS_OS232_WINDOWS16 = 0x00030001,
            VOS_OS232_WINDOWS32 = 0x00030004,
            VOS_OS232_PM16      = 0x00030002,
            VOS_OS232_PM32      = 0x00030003
        }

        /// <summary>
        ///     Version file subtype.
        /// </summary>
        public enum VersionFileSubtype : uint
        {
            VFT2_UNKNOWN = 0x00000000,
            // Drivers
            VFT2_DRV_COMM              = 0x0000000A,
            VFT2_DRV_DISPLAY           = 0x00000004,
            VFT2_DRV_INSTALLABLE       = 0x00000008,
            VFT2_DRV_KEYBOARD          = 0x00000002,
            VFT2_DRV_LANGUAGE          = 0x00000003,
            VFT2_DRV_MOUSE             = 0x00000005,
            VFT2_DRV_NETWORK           = 0x00000006,
            VFT2_DRV_PRINTER           = 0x00000001,
            VFT2_DRV_SOUND             = 0x00000009,
            VFT2_DRV_SYSTEM            = 0x00000007,
            VFT2_DRV_VERSIONED_PRINTER = 0x0000000C,
            // Fonts
            VFT2_FONT_RASTER   = 0x00000001,
            VFT2_FONT_TRUETYPE = 0x00000003,
            VFT2_FONT_VECTOR   = 0x00000002
        }

        /// <summary>
        ///     Version file type.
        /// </summary>
        public enum VersionFileType : uint
        {
            VFT_APP        = 0x00000001,
            VFT_DLL        = 0x00000002,
            VFT_DRV        = 0x00000003,
            VFT_FONT       = 0x00000004,
            VFT_STATIC_LIB = 0x00000007,
            VFT_UNKNOWN    = 0x00000000,
            VFT_VXD        = 0x00000005
        }
   }
}