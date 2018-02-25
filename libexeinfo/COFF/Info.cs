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
using System.Text;

namespace libexeinfo
{
    public partial class COFF
    {
        public string Information => GetInfo(Header);

        internal static string GetInfo(COFFHeader header)
        {
            DateTime      epoch = new DateTime(1970, 1, 1);
            StringBuilder sb    = new StringBuilder();
            sb.AppendLine("Common Object File Format (COFF):");
            switch(header.optionalHeader.magic)
            {
                case JMAGIC:
                    sb.AppendLine("\tExecutable is dirty and unshareable");
                    break;
                case DMAGIC:
                    sb.AppendLine("\tExecutable is dirty, data is aligned");
                    break;
                case ZMAGIC:
                    sb.AppendLine("\tNormal executable");
                    break;
                case SHMAGIC:
                    sb.AppendLine("\tShared library");
                    break;
                case PE.PE32Plus:
                    sb.AppendLine("\tPE32+ executable");
                    break;
                default:
                    sb.AppendFormat("\tUnknown executable type with magic {0}", header.optionalHeader.magic)
                      .AppendLine();
                    break;
            }

            sb.AppendFormat("\tMachine: {0}", MachineTypeToString(header.machine)).AppendLine();

            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_RELOCS_STRIPPED))
                sb.AppendLine("\tExecutable contains no relocations.");
            sb.AppendLine(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_EXECUTABLE_IMAGE)
                              ? "\tExecutable is valid."
                              : "\tExecutable is invalid, contains errors or has not been linked correctly.");
            if(!header.characteristics.HasFlag(Characteristics.IMAGE_FILE_LINE_NUMS_STRIPPED))
                sb.AppendLine("\tExecutable contains line numbers.");
            if(!header.characteristics.HasFlag(Characteristics.IMAGE_FILE_LOCAL_SYMS_STRIPPED))
                sb.AppendLine("\tExecutable contains debug symbols.");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_AGGRESSIVE_WS_TRIM))
                sb.AppendLine("\tWorking set should be aggressively trimmed");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_LARGE_ADDRESS_AWARE))
                sb.AppendLine("\tExecutable can handle addresses bigger than 2GiB");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_16BIT_MACHINE))
                sb.AppendLine("\tExecutable is for a 16-bit per word machine");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_BYTES_REVERSED_LO))
                sb.AppendLine("\tExecutable is little-endian");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_32BIT_MACHINE))
                sb.AppendLine("\tExecutable is for a 32-bit per word machine");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_DEBUG_STRIPPED))
                sb.AppendLine("\tDebug information has been removed");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP))
                sb.AppendLine("\tWhen executable is run from removable media, it should be copied to swap and run from there");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_NET_RUN_FROM_SWAP))
                sb.AppendLine("\tWhen executable is run from a network share, it should be copied to swap and run from there");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_SYSTEM))
                sb.AppendLine("\tExecutable is a system file");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_DLL))
                sb.AppendLine("\tExecutable is a dynamically linkable library");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_UP_SYSTEM_ONLY))
                sb.AppendLine("\tExecutable can only run on uniprocessor machines");
            if(header.characteristics.HasFlag(Characteristics.IMAGE_FILE_BYTES_REVERSED_HI))
                sb.AppendLine("\tExecutable is big-endian");

            sb.AppendFormat("\tExecutable has {0} sections",  header.numberOfSections).AppendLine();
            sb.AppendFormat("\tExecutable was linked on {0}", epoch.AddSeconds(header.timeDateStamp)).AppendLine();
            sb.AppendFormat("\tSymbol table starts at {0} and contains {1} symbols", header.pointerToSymbolTable,
                            header.numberOfSymbols).AppendLine();
            sb.AppendFormat("\tOptional header has {0} bytes", header.sizeOfOptionalHeader).AppendLine();
            if(header.optionalHeader.majorLinkerVersion > 0 || header.optionalHeader.minorLinkerVersion > 0)
                sb.AppendFormat("\tLinker version: {0}.{1}", header.optionalHeader.majorLinkerVersion,
                                header.optionalHeader.minorLinkerVersion).AppendLine();
            sb.AppendFormat("\tCode has {0} bytes",             header.optionalHeader.sizeOfCode).AppendLine();
            sb.AppendFormat("\tInitialized data has {0} bytes", header.optionalHeader.sizeOfInitializedData)
              .AppendLine();
            sb.AppendFormat("\tUninitialized data has {0} bytes", header.optionalHeader.sizeOfUninitializedData)
              .AppendLine();
            sb.AppendFormat("\tAddress point starts at {0}", header.optionalHeader.addressOfEntryPoint).AppendLine();
            sb.AppendFormat("\tCode starts at {0}",          header.optionalHeader.baseOfCode).AppendLine();
            if(header.optionalHeader.magic != PE.PE32Plus)
                sb.AppendFormat("\tData starts at {0}", header.optionalHeader.baseOfData).AppendLine();
            return sb.ToString();
        }
    }
}