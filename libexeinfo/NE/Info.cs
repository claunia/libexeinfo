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
	public partial class NE
	{
		public static string GetInfo(NEHeader header)
		{
            StringBuilder sb = new StringBuilder();
			sb.AppendLine("New Executable (NE):");
			sb.AppendFormat("\tFile's CRC: 0x{0:X8}", header.crc).AppendLine();
			sb.AppendFormat("\tLinker version: {0}.{1}", header.linker_major, header.linker_minor).AppendLine();
			if (header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && !header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				sb.AppendLine("\tApplication uses a single shared DGroup");
			else if (!header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				sb.AppendLine("\tApplication uses a multiple DGroup");
			else if (header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				sb.AppendLine("\tApplication indicates an incorrect DGroup value");
			else if (!header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && !header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				sb.AppendLine("\tApplication does not use DGroup");
			if (header.program_flags.HasFlag(ProgramFlags.GlobalInit))
				sb.AppendLine("\tApplication uses global initialization");
			if (header.program_flags.HasFlag(ProgramFlags.ProtectedMode))
				sb.AppendLine("\tApplication uses protected mode");
			if (header.program_flags.HasFlag(ProgramFlags.i86))
				sb.AppendLine("\tApplication uses 8086 instructions");
			if (header.program_flags.HasFlag(ProgramFlags.i286))
				sb.AppendLine("\tApplication uses 80286 instructions");
			if (header.program_flags.HasFlag(ProgramFlags.i386))
				sb.AppendLine("\tApplication uses 80386 instructions");
			if (header.program_flags.HasFlag(ProgramFlags.i87))
				sb.AppendLine("\tApplication uses floating point instructions");

			if (header.target_os == TargetOS.OS2)
			{
				sb.AppendLine("\tOS/2 application");
				sb.AppendFormat("\tApplication requires OS/2 {0}.{1} to run", header.os_major, header.os_minor).AppendLine();
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is full screen, unaware of Presentation Manager");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is aware of Presentation Manager, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication uses Presentation Manager");
				if (header.os2_flags.HasFlag(OS2Flags.LongFilename))
					sb.AppendLine("\tApplication supports long filenames");
				if (header.os2_flags.HasFlag(OS2Flags.ProtectedMode2))
					sb.AppendLine("\tApplication uses OS/2 2.x protected mode");
				if (header.os2_flags.HasFlag(OS2Flags.ProportionalFonts))
					sb.AppendLine("\tApplication uses OS/2 2.x proportional fonts");
				if (header.os2_flags.HasFlag(OS2Flags.GangloadArea))
					sb.AppendFormat("\tGangload area starts at {0} an runs for {1} bytes", header.return_thunks_offset, header.segment_reference_thunks).AppendLine();
				else
				{
					sb.AppendFormat("\tReturn thunks are at: {0}", header.return_thunks_offset).AppendLine();
					sb.AppendFormat("\tSegment reference thunks are at: {0}", header.segment_reference_thunks).AppendLine();
				}
			}
			else if (header.target_os == TargetOS.Windows || header.target_os == TargetOS.Win32)
			{
				if (header.target_os == TargetOS.Windows)
					sb.AppendLine("\t16-bit Windows application");
				else if (header.target_os == TargetOS.Win32)
					sb.AppendLine("\t32-bit Windows application");
				sb.AppendFormat("\tApplication requires Windows {0}.{1} to run", header.os_major, header.os_minor).AppendLine();
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is full screen, unaware of Windows");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication uses Windows");
				sb.AppendFormat("\tReturn thunks are at: {0}", header.return_thunks_offset).AppendLine();
				sb.AppendFormat("\tSegment reference thunks are at: {0}", header.segment_reference_thunks).AppendLine();
			}
			else if (header.target_os == TargetOS.DOS)
			{
				sb.AppendLine("\tDOS application");
				sb.AppendFormat("\tApplication requires DOS {0}.{1} to run", header.os_major, header.os_minor).AppendLine();
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is full screen, unaware of Windows");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication uses Windows");
				sb.AppendFormat("\tReturn thunks are at: {0}", header.return_thunks_offset).AppendLine();
				sb.AppendFormat("\tSegment reference thunks are at: {0}", header.segment_reference_thunks).AppendLine();
			}
			else if (header.target_os == TargetOS.Borland)
			{
				sb.AppendLine("\tBorland Operating System Services application");
				sb.AppendFormat("\tApplication requires DOS {0}.{1} to run", header.os_major, header.os_minor).AppendLine();
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is full screen, unaware of Windows");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication is aware of Windows, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					sb.AppendLine("\tApplication uses Windows");
				sb.AppendFormat("\tReturn thunks are at: {0}", header.return_thunks_offset).AppendLine();
				sb.AppendFormat("\tSegment reference thunks are at: {0}", header.segment_reference_thunks).AppendLine();
			}
			else
			{
				sb.AppendFormat("\tApplication for unknown OS {0}", (byte)header.target_os).AppendLine();
				sb.AppendFormat("\tApplication requires OS {0}.{1} to run", header.os_major, header.os_minor).AppendLine();
				sb.AppendFormat("\tReturn thunks are at: {0}", header.return_thunks_offset).AppendLine();
				sb.AppendFormat("\tSegment reference thunks are at: {0}", header.segment_reference_thunks).AppendLine();
			}

			if (header.application_flags.HasFlag(ApplicationFlags.Errors))
				sb.AppendLine("\tExecutable has errors");
			if (header.application_flags.HasFlag(ApplicationFlags.NonConforming))
				sb.AppendLine("\tExecutable is non conforming");
			if (header.application_flags.HasFlag(ApplicationFlags.DLL))
				sb.AppendLine("\tExecutable is a dynamic library or a driver");

			sb.AppendFormat("\tMinimum code swap area: {0} bytes", header.minimum_swap_area).AppendLine();
			sb.AppendFormat("\tFile alignment shift: {0}", 512 << header.alignment_shift).AppendLine();
			sb.AppendFormat("\tInitial local heap should be {0} bytes", header.initial_heap).AppendLine();
			sb.AppendFormat("\tInitial stack size should be {0} bytes", header.initial_stack).AppendLine();
			sb.AppendFormat("\tCS:IP entry point: {0:X4}:{1:X4}", (header.entry_point & 0xFFFF0000) >> 16, header.entry_point & 0xFFFF).AppendLine();
			if (!header.application_flags.HasFlag(ApplicationFlags.DLL))
				sb.AppendFormat("\tSS:SP initial stack pointer: {0:X4}:{1:X4}", (header.stack_pointer & 0xFFFF0000) >> 16, header.stack_pointer & 0xFFFF).AppendLine();
			sb.AppendFormat("\tEntry table starts at {0} and runs for {1} bytes", header.entry_table_offset, header.entry_table_length).AppendLine();
			sb.AppendFormat("\tSegment table starts at {0} and contain {1} segments", header.segment_table_offset, header.segment_count).AppendLine();
			sb.AppendFormat("\tModule reference table starts at {0} and contain {1} references", header.module_reference_offset, header.reference_count).AppendLine();
			sb.AppendFormat("\tNon-resident names table starts at {0} and runs for {1} bytes", header.nonresident_names_offset, header.nonresident_table_size).AppendLine();
			sb.AppendFormat("\tResources table starts at {0} and contains {1} entries", header.resource_table_offset, header.resource_entries).AppendLine();
			sb.AppendFormat("\tResident names table starts at {0}", header.resident_names_offset).AppendLine();
			sb.AppendFormat("\tImported names table starts at {0}", header.imported_names_offset).AppendLine();
            return sb.ToString();
		}

        public string GetInfo()
        {
            return GetInfo(Header);
        }

        public static ResourceTable GetResources(FileStream stream, uint neStart, ushort tableOff)
		{
			long oldPosition = stream.Position;
			byte[] DW = new byte[2];
			byte[] DD = new byte[4];

			stream.Position = neStart + tableOff;
			ResourceTable table = new ResourceTable();
			stream.Read(DW, 0, 2);
			table.alignment_shift = BitConverter.ToUInt16(DW, 0);

			List<ResourceType> types = new List<ResourceType>();

			while (true)
			{
				ResourceType type = new ResourceType();
				stream.Read(DW, 0, 2);
				type.id = BitConverter.ToUInt16(DW, 0);
				if (type.id == 0)
					break;

				stream.Read(DW, 0, 2);
				type.count = BitConverter.ToUInt16(DW, 0);
				stream.Read(DD, 0, 4);
				type.reserved = BitConverter.ToUInt32(DD, 0);

				type.resources = new Resource[type.count];
				for (int i = 0; i < type.count; i++)
				{
					type.resources[i] = new Resource();
					stream.Read(DW, 0, 2);
					type.resources[i].dataOffset = BitConverter.ToUInt16(DW, 0);
					stream.Read(DW, 0, 2);
					type.resources[i].length = BitConverter.ToUInt16(DW, 0);
					stream.Read(DW, 0, 2);
					type.resources[i].flags = (ResourceFlags)BitConverter.ToUInt16(DW, 0);
					stream.Read(DW, 0, 2);
					type.resources[i].id = BitConverter.ToUInt16(DW, 0);
					stream.Read(DD, 0, 4);
					type.resources[i].reserved = BitConverter.ToUInt32(DD, 0);
				}

				types.Add(type);
			}

			table.types = types.ToArray();

			for (int t = 0; t < table.types.Length; t++)
			{
				if ((table.types[t].id & 0x8000) == 0)
				{
					byte len;
					byte[] str;
					stream.Position = neStart + tableOff + table.types[t].id;
					len = (byte)stream.ReadByte();
					str = new byte[len];
					stream.Read(str, 0, len);
					table.types[t].name = Encoding.ASCII.GetString(str);
				}
				else
					table.types[t].name = ResourceIdToName(table.types[t].id);

				for (int r = 0; r < table.types[t].resources.Length; r++)
				{
					if ((table.types[t].resources[r].id & 0x8000) == 0)
					{
						byte len;
						byte[] str;
						stream.Position = neStart + tableOff + table.types[t].resources[r].id;
						len = (byte)stream.ReadByte();
						str = new byte[len];
						stream.Read(str, 0, len);
						table.types[t].resources[r].name = Encoding.ASCII.GetString(str);
					}
					else
						table.types[t].resources[r].name = string.Format("{0}", table.types[t].resources[r].id & 0x7FFF);

					table.types[t].resources[r].data = new byte[table.types[t].resources[r].length * (1 << table.alignment_shift)];
					stream.Position = table.types[t].resources[r].dataOffset * (1 << table.alignment_shift);
					stream.Read(table.types[t].resources[r].data, 0, table.types[t].resources[r].data.Length);
				}
			}

			stream.Position = oldPosition;

			return table;
		}
	}
}
