//
// Subsystems.cs
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

namespace libexeinfo
{
    public partial class PE
    {
        static string SubsystemToString(Subsystems subsystem)
        {
            switch(subsystem)
            {
                case Subsystems.IMAGE_SUBSYSTEM_UNKNOWN:     return "for an unknown subsystem with no code";
                case Subsystems.IMAGE_SUBSYSTEM_NATIVE:      return "an application for native Windows API";
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_GUI: return "an application for Windows GUI";
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_CUI:
                    return "an application for Windows command-line";
                case Subsystems.IMAGE_SUBSYSTEM_OS2_CUI:                 return "an application for OS/2 command-line";
                case Subsystems.IMAGE_SUBSYSTEM_POSIX_CUI:               return "an application for POSIX command-line";
                case Subsystems.IMAGE_SUBSYSTEM_NATIVE_WINDOWS:          return "a driver for Windows 9x";
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_CE_GUI:          return "an application for Windows CE";
                case Subsystems.IMAGE_SUBSYSTEM_EFI_APPLICATION:         return "an EFI application";
                case Subsystems.IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER: return "an EFI boot services driver";
                case Subsystems.IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER:      return "an EFI run-time services driver";
                case Subsystems.IMAGE_SUBSYSTEM_EFI_ROM:                 return "an EFI ROM image";
                case Subsystems.IMAGE_SUBSYSTEM_XBOX:                    return "a Xbox executable";
                case Subsystems.IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION:
                    return "an application for Windows Boot environment";
                default: return $"for an unknown subsystem with code {(ushort)subsystem}";
            }
        }
    }
}