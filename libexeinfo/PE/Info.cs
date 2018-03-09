//
// Info.cs
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

using System.Linq;
using System.Text;

namespace libexeinfo
{
    public partial class PE
    {
        public string Information
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(baseExecutable.Information);
                sb.Append(COFF.GetInfo(header.coff));
                sb.AppendLine("Portable Executable (PE):");
                if(!string.IsNullOrEmpty(moduleName)) sb.AppendFormat("\tModule name: {0}", moduleName).AppendLine();
                if(header.coff.optionalHeader.magic == PE32Plus)
                    sb.AppendFormat("\tExecutable base address: 0x{0:X16}", winHeader.imageBase).AppendLine();
                else sb.AppendFormat("\tExecutable base address: 0x{0:X8}", winHeader.imageBase).AppendLine();
                sb.AppendFormat("\tSections are aligned to {0} bytes", winHeader.sectionAlignment).AppendLine();
                sb.AppendFormat("\tFile is aligned to {0} bytes", winHeader.fileAlignment).AppendLine();
                if(winHeader.majorOperatingSystemVersion > 0 || winHeader.minorOperatingSystemVersion > 0)
                    sb.AppendFormat("\tExecutable requires at least operating system version {0}.{1} to run",
                                    winHeader.majorOperatingSystemVersion, winHeader.minorOperatingSystemVersion)
                      .AppendLine();
                if(winHeader.majorImageVersion > 0 || winHeader.minorImageVersion > 0)
                    sb.AppendFormat("\tExecutable version: {0}.{1}", winHeader.majorImageVersion,
                                    winHeader.minorImageVersion).AppendLine();
                sb.AppendFormat("\tAccording to subsystem, executable is {0}", SubsystemToString(winHeader.subsystem))
                  .AppendLine();
                if(importedNames != null && importedNames.Contains("libbe.so"))
                    sb.AppendLine("\tExecutable is a BeOS R3 executable.");
                if(importedNames != null && importedNames.Contains("Singularity.V1.dll"))
                    sb.AppendLine("\tExecutable is a Singularity executable.");
                if(winHeader.majorSubsystemVersion > 0 || winHeader.minorSubsystemVersion > 0)
                    sb.AppendFormat("\tExecutable requires at least subsystem version {0}.{1} to run",
                                    winHeader.majorSubsystemVersion, winHeader.minorSubsystemVersion).AppendLine();

                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_HIGH_ENTROPY_VA))
                    sb.AppendLine("\tExecutable can handle a high entropy 64-bit virtual address space");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE))
                    sb.AppendLine("\tExecutable can be relocated at load time");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_FORCE_INTEGRITY))
                    sb.AppendLine("\tCode Integrity checks are enforced");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NX_COMPAT))
                    sb.AppendLine("\tExecutable is NX compatible");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_ISOLATION))
                    sb.AppendLine("\tExecutable is isolation aware, but should not be isolated");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_SEH))
                    sb.AppendLine("\tExecutable does not use structured exception handling");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_BIND))
                    sb.AppendLine("\tExecutable should not be binded");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_APPCONTAINER))
                    sb.AppendLine("\tExecutable must be run inside an AppContainer");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_WDM_DRIVER))
                    sb.AppendLine("\tExecutable contains a WDM driver");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics.IMAGE_DLLCHARACTERISTICS_GUARD_CF))
                    sb.AppendLine("\tExecutable supports Control Flow Guard");
                if(winHeader.dllCharacteristics.HasFlag(DllCharacteristics
                                                           .IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE))
                    sb.AppendLine("\tExecutable is Terminal Server aware");

                if(winHeader.win32VersionValue > 0)
                    sb.AppendFormat("\tWin32 version value: {0}", winHeader.win32VersionValue).AppendLine();
                sb.AppendFormat("\tExecutable is {0} bytes", winHeader.sizeOfImage).AppendLine();
                sb.AppendFormat("\tHeaders are {0} bytes", winHeader.sizeOfHeaders).AppendLine();
                sb.AppendFormat("\tChecksum: 0x{0:X8}", winHeader.checksum).AppendLine();
                sb.AppendFormat("\t{0} bytes of stack should be reserved", winHeader.sizeOfStackReserve).AppendLine();
                sb.AppendFormat("\t{0} bytes of stack should be committed", winHeader.sizeOfStackCommit).AppendLine();
                sb.AppendFormat("\t{0} bytes of heap should be reserved", winHeader.sizeOfHeapReserve).AppendLine();
                sb.AppendFormat("\t{0} bytes of heap should be committed", winHeader.sizeOfHeapCommit).AppendLine();
                if(winHeader.loaderFlags > 0)
                    sb.AppendFormat("\tLoader flags: {0}", winHeader.loaderFlags).AppendLine();
                sb.AppendFormat("\t{0} RVA entries follow the header", winHeader.numberOfRvaAndSizes).AppendLine();

                for(int i = 0; i < directoryEntries.Length; i++)
                {
                    string tableName;
                    switch(i)
                    {
                        case 0:
                            tableName = "Export table";
                            break;
                        case 1:
                            tableName = "Import table";
                            break;
                        case 2:
                            tableName = "Resource table";
                            break;
                        case 3:
                            tableName = "Exception table";
                            break;
                        case 4:
                            tableName = "Certificate table";
                            break;
                        case 5:
                            tableName = "Base relocation table";
                            break;
                        case 6:
                            tableName = "Debug data";
                            break;
                        case 7:
                            tableName = "Architecture-specific data";
                            break;
                        case 8:
                            tableName = "Global pointer register";
                            break;
                        case 9:
                            tableName = "Thread local storage table";
                            break;
                        case 10:
                            tableName = "Load configuration table";
                            break;
                        case 11:
                            tableName = "Bound import table";
                            break;
                        case 12:
                            tableName = "Import address table";
                            break;
                        case 13:
                            tableName = "Delay import descriptor";
                            break;
                        case 14:
                            tableName = "CLR runtime header";
                            break;
                        default:
                            tableName = "Unknown table";
                            break;
                    }

                    if(directoryEntries[i].rva == 0)
                        sb.AppendFormat("\tImage does not contain {0}", tableName).AppendLine();
                    else
                        sb.AppendFormat("\t{0} starts at virtual address {1} ({2} physical offset) and has {3} bytes",
                                        tableName, directoryEntries[i].rva,
                                        RvaToReal(directoryEntries[i].rva, sectionHeaders), directoryEntries[i].size)
                          .AppendLine();
                }

                sb.AppendFormat("\t{0} sections:", sectionHeaders.Length).AppendLine();
                for(int i = 0; i < sectionHeaders.Length; i++)
                {
                    sb.AppendFormat("\t\tSection {0}:", i).AppendLine();
                    sb.AppendFormat("\t\t\tName: {0}", sectionHeaders[i].name).AppendLine();
                    sb.AppendFormat("\t\t\tCharacteristics: {0}", sectionHeaders[i].characteristics).AppendLine();
                    sb.AppendFormat("\t\t\t{0} relocations start at {1}", sectionHeaders[i].numberOfRelocations,
                                    sectionHeaders[i].pointerToRelocations).AppendLine();
                    sb.AppendFormat("\t\t\t{0} line numbers start at {1}", sectionHeaders[i].numberOfLineNumbers,
                                    sectionHeaders[i].pointerToRelocations).AppendLine();
                    sb.AppendFormat("\t\t\tRaw data starts at {0} and has {1} bytes",
                                    sectionHeaders[i].pointerToRawData, sectionHeaders[i].sizeOfRawData).AppendLine();
                    sb.AppendFormat("\t\t\tVirtual address: {0}", sectionHeaders[i].virtualAddress).AppendLine();
                    sb.AppendFormat("\t\t\tVirtual size: {0} bytes", sectionHeaders[i].virtualSize).AppendLine();
                }

                if(exportedNames != null)
                {
                    sb.AppendLine("\tExported functions:");
                    foreach(string name in exportedNames) sb.AppendFormat("\t\t{0}", name).AppendLine();
                }

                if(importedNames != null)
                {
                    sb.AppendLine("\tImported libraries:");
                    foreach(string name in importedNames) sb.AppendFormat("\t\t{0}", name).AppendLine();
                }

                if(debugDirectory.pointerToRawData > 0)
                    sb.AppendFormat("\tExecutable contains debug information of type {0}", debugDirectory.type)
                      .AppendLine();

                if(WindowsResourcesRoot != null)
                {
                    sb.AppendLine("\tResources:");
                    PrintResourceNode(sb, WindowsResourcesRoot, 2);
                }

                return sb.ToString();
            }
        }

        static void PrintResourceNode(StringBuilder sb, ResourceNode node, int level)
        {
            for(int i = 0; i < level; i++) sb.Append("\t");
            if(node.children != null)
            {
                switch(node.level)
                {
                    case 0:
                        sb.AppendFormat("Root contains {0} types:", node.children.Length).AppendLine();
                        break;
                    case 1:
                        sb.AppendFormat("Type {0} has {1} items:", node.name, node.children.Length).AppendLine();
                        break;
                    case 2:
                        sb.AppendFormat("ID {0} has {1} languages:", node.id, node.children.Length).AppendLine();
                        break;
                    default:
                        sb.AppendFormat("ID {0} has {1} items:", node.id, node.children.Length).AppendLine();
                        break;
                }

                foreach(ResourceNode child in node.children) PrintResourceNode(sb, child, level + 1);
            }

            if(node.data == null) return;

            if(node.level == 3)
                sb.AppendFormat("{0} contains {1} bytes.", node.name, node.data.Length).AppendLine();
            else sb.AppendFormat("ID {0} contains {1} bytes.", node.id, node.data.Length).AppendLine();
        }
    }
}