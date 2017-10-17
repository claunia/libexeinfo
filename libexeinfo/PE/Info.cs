//
// Info.cs
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
using System.Text;

namespace libexeinfo
{
	public partial class PE
	{
        public static string GetInfo(PEHeader header, WindowsHeader64 winheader)
		{
            StringBuilder sb = new StringBuilder();
            sb.Append(COFF.GetInfo(header.coff));
			sb.AppendLine("Portable Executable (PE):");

            if(header.coff.optionalHeader.magic == PE32Plus)
                sb.AppendFormat("\tExecutable base address: 0x{0:X16}", winheader.imageBase).AppendLine();
            else
                sb.AppendFormat("\tExecutable base address: 0x{0:X8}", winheader.imageBase).AppendLine();
			sb.AppendFormat("\tSections are aligned to {0} bytes", winheader.sectionAlignment).AppendLine();
			sb.AppendFormat("\tFile is aligned to {0} bytes", winheader.fileAlignment).AppendLine();
            if(winheader.majorOperatingSystemVersion > 0 || winheader.minorOperatingSystemVersion > 0)
                sb.AppendFormat("\tExecutable requires at least operating system version {0}.{1} to run", winheader.majorOperatingSystemVersion, winheader.minorOperatingSystemVersion).AppendLine();
			if (winheader.majorImageVersion > 0 || winheader.minorImageVersion > 0)
                sb.AppendFormat("\tExecutable version: {0}.{1}", winheader.majorImageVersion, winheader.minorImageVersion).AppendLine();
            sb.AppendFormat("\tAccording to subsystem, executable is {0}", SubsystemToString(winheader.subsystem)).AppendLine();
			if (winheader.majorSubsystemVersion > 0 || winheader.minorSubsystemVersion > 0)
                sb.AppendFormat("\tExecutable requires at least subsystem version {0}.{1} to run", winheader.majorSubsystemVersion, winheader.minorSubsystemVersion).AppendLine();

            if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_HIGH_ENTROPY_VA))
                sb.AppendLine("\tExecutable can handle a high entropy 64-bit virtual address space");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE))
				sb.AppendLine("\tExecutable can be relocated at load time");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_FORCE_INTEGRITY))
				sb.AppendLine("\tCode Integrity checks are enforced");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NX_COMPAT))
				sb.AppendLine("\tExecutable is NX compatible");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_ISOLATION))
				sb.AppendLine("\tExecutable is isolation aware, but should not be isolated");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_SEH))
				sb.AppendLine("\tExecutable does not use structured exception handling");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_BIND))
				sb.AppendLine("\tExecutable should not be binded");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_APPCONTAINER))
				sb.AppendLine("\tExecutable must be run inside an AppContainer");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_WDM_DRIVER))
				sb.AppendLine("\tExecutable contains a WDM driver");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_GUARD_CF))
				sb.AppendLine("\tExecutable supports Control Flow Guard");
			if (winheader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE))
				sb.AppendLine("\tExecutable is Terminal Server aware");

			if (winheader.win32VersionValue > 0)
                sb.AppendFormat("\tWin32 version value: {0}", winheader.win32VersionValue).AppendLine();
			sb.AppendFormat("\tExecutable is {0} bytes", winheader.sizeOfImage).AppendLine();
			sb.AppendFormat("\tHeaders are {0} bytes", winheader.sizeOfHeaders).AppendLine();
            sb.AppendFormat("\tChecksum: 0x{0:X8}", winheader.checksum).AppendLine();
			sb.AppendFormat("\t{0} bytes of stack should be reserved", winheader.sizeOfStackReserve).AppendLine();
			sb.AppendFormat("\t{0} bytes of stack should be committed", winheader.sizeOfStackCommit).AppendLine();
			sb.AppendFormat("\t{0} bytes of heap should be reserved", winheader.sizeOfHeapReserve).AppendLine();
			sb.AppendFormat("\t{0} bytes of heap should be committed", winheader.sizeOfHeapCommit).AppendLine();
            if (winheader.loaderFlags > 0)
                sb.AppendFormat("\tLoader flags: {0}", winheader.loaderFlags).AppendLine();
			sb.AppendFormat("\t{0} RVA entries follow the header", winheader.numberOfRvaAndSizes).AppendLine();

			return sb.ToString();
		}

        public string GetInfo()
        {
            return GetInfo(Header, WinHeader);
        }
	}
}
