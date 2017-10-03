using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace exeinfo.NE
{
    public class Info
    {
		public static void PrintInfo(Header header)
		{
			Console.WriteLine("New Executable (NE):");
			Console.WriteLine("\tFile's CRC: 0x{0:X8}", header.crc);
			Console.WriteLine("\tLinker version: {0}.{1}", header.linker_major, header.linker_minor);
			if (header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && !header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				Console.WriteLine("\tApplication uses a single shared DGroup");
			else if (!header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				Console.WriteLine("\tApplication uses a multiple DGroup");
			else if (header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				Console.WriteLine("\tApplication indicates an incorrect DGroup value");
			else if (!header.program_flags.HasFlag(ProgramFlags.SingleDGroup) && !header.program_flags.HasFlag(ProgramFlags.MultipleDGroup))
				Console.WriteLine("\tApplication does not use DGroup");
			if (header.program_flags.HasFlag(ProgramFlags.GlobalInit))
				Console.WriteLine("\tApplication uses global initialization");
			if (header.program_flags.HasFlag(ProgramFlags.ProtectedMode))
				Console.WriteLine("\tApplication uses protected mode");
			if (header.program_flags.HasFlag(ProgramFlags.i86))
				Console.WriteLine("\tApplication uses 8086 instructions");
			if (header.program_flags.HasFlag(ProgramFlags.i286))
				Console.WriteLine("\tApplication uses 80286 instructions");
			if (header.program_flags.HasFlag(ProgramFlags.i386))
				Console.WriteLine("\tApplication uses 80386 instructions");
			if (header.program_flags.HasFlag(ProgramFlags.i87))
				Console.WriteLine("\tApplication uses floating point instructions");

			if (header.target_os == TargetOS.OS2)
			{
				Console.WriteLine("\tOS/2 application");
				Console.WriteLine("\tApplication requires OS/2 {0}.{1} to run", header.os_major, header.os_minor);
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is full screen, unaware of Presentation Manager");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is aware of Presentation Manager, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication uses Presentation Manager");
				if (header.os2_flags.HasFlag(OS2Flags.LongFilename))
					Console.WriteLine("\tApplication supports long filenames");
				if (header.os2_flags.HasFlag(OS2Flags.ProtectedMode2))
					Console.WriteLine("\tApplication uses OS/2 2.x protected mode");
				if (header.os2_flags.HasFlag(OS2Flags.ProportionalFonts))
					Console.WriteLine("\tApplication uses OS/2 2.x proportional fonts");
				if (header.os2_flags.HasFlag(OS2Flags.GangloadArea))
					Console.WriteLine("\tGangload area starts at {0} an runs for {1} bytes", header.return_thunks_offset, header.segment_reference_thunks);
				else
				{
					Console.WriteLine("\tReturn thunks are at: {0}", header.return_thunks_offset);
					Console.WriteLine("\tSegment reference thunks are at: {0}", header.segment_reference_thunks);
				}
			}
			else if (header.target_os == TargetOS.Windows || header.target_os == TargetOS.Win32)
			{
				if (header.target_os == TargetOS.Windows)
					Console.WriteLine("\t16-bit Windows application");
				else if (header.target_os == TargetOS.Win32)
					Console.WriteLine("\t32-bit Windows application");
				Console.WriteLine("\tApplication requires Windows {0}.{1} to run", header.os_major, header.os_minor);
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is full screen, unaware of Windows");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is aware of Windows, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication uses Windows");
				Console.WriteLine("\tReturn thunks are at: {0}", header.return_thunks_offset);
				Console.WriteLine("\tSegment reference thunks are at: {0}", header.segment_reference_thunks);
			}
			else if (header.target_os == TargetOS.DOS)
			{
				Console.WriteLine("\tDOS application");
				Console.WriteLine("\tApplication requires DOS {0}.{1} to run", header.os_major, header.os_minor);
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is full screen, unaware of Windows");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is aware of Windows, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication uses Windows");
				Console.WriteLine("\tReturn thunks are at: {0}", header.return_thunks_offset);
				Console.WriteLine("\tSegment reference thunks are at: {0}", header.segment_reference_thunks);
			}
			else if (header.target_os == TargetOS.Borland)
			{
				Console.WriteLine("\tBorland Operating System Services application");
				Console.WriteLine("\tApplication requires DOS {0}.{1} to run", header.os_major, header.os_minor);
				if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && !header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is full screen, unaware of Windows");
				else if (!header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication is aware of Windows, but doesn't use it");
				else if (header.application_flags.HasFlag(ApplicationFlags.FullScreen) && header.application_flags.HasFlag(ApplicationFlags.GUICompatible))
					Console.WriteLine("\tApplication uses Windows");
				Console.WriteLine("\tReturn thunks are at: {0}", header.return_thunks_offset);
				Console.WriteLine("\tSegment reference thunks are at: {0}", header.segment_reference_thunks);
			}
			else
			{
				Console.WriteLine("\tApplication for unknown OS {0}", (byte)header.target_os);
				Console.WriteLine("\tApplication requires OS {0}.{1} to run", header.os_major, header.os_minor);
				Console.WriteLine("\tReturn thunks are at: {0}", header.return_thunks_offset);
				Console.WriteLine("\tSegment reference thunks are at: {0}", header.segment_reference_thunks);
			}

			if (header.application_flags.HasFlag(ApplicationFlags.Errors))
				Console.WriteLine("\tExecutable has errors");
			if (header.application_flags.HasFlag(ApplicationFlags.NonConforming))
				Console.WriteLine("\tExecutable is non conforming");
			if (header.application_flags.HasFlag(ApplicationFlags.DLL))
				Console.WriteLine("\tExecutable is a dynamic library or a driver");

			Console.WriteLine("\tMinimum code swap area: {0} bytes", header.minimum_swap_area);
			Console.WriteLine("\tFile alignment shift: {0}", 512 << header.alignment_shift);
			Console.WriteLine("\tInitial local heap should be {0} bytes", header.initial_heap);
			Console.WriteLine("\tInitial stack size should be {0} bytes", header.initial_stack);
			Console.WriteLine("\tCS:IP entry point: {0:X4}:{1:X4}", (header.entry_point & 0xFFFF0000) >> 16, header.entry_point & 0xFFFF);
			if (!header.application_flags.HasFlag(ApplicationFlags.DLL))
				Console.WriteLine("\tSS:SP initial stack pointer: {0:X4}:{1:X4}", (header.stack_pointer & 0xFFFF0000) >> 16, header.stack_pointer & 0xFFFF);
			Console.WriteLine("\tEntry table starts at {0} and runs for {1} bytes", header.entry_table_offset, header.entry_table_length);
			Console.WriteLine("\tSegment table starts at {0} and contain {1} segments", header.segment_table_offset, header.segment_count);
			Console.WriteLine("\tModule reference table starts at {0} and contain {1} references", header.module_reference_offset, header.reference_count);
			Console.WriteLine("\tNon-resident names table starts at {0} and runs for {1} bytes", header.nonresident_names_offset, header.nonresident_table_size);
			Console.WriteLine("\tResources table starts at {0} and contains {1} entries", header.resource_table_offset, header.resource_entries);
			Console.WriteLine("\tResident names table starts at {0}", header.resident_names_offset);
			Console.WriteLine("\tImported names table starts at {0}", header.imported_names_offset);
		}

		public static ResourceTable GetResources(FileStream exeFs, uint neStart, ushort tableOff)
		{
			long oldPosition = exeFs.Position;
			byte[] DW = new byte[2];
			byte[] DD = new byte[4];

			exeFs.Position = neStart + tableOff;
			ResourceTable table = new ResourceTable();
			exeFs.Read(DW, 0, 2);
			table.alignment_shift = BitConverter.ToUInt16(DW, 0);

			List<ResourceType> types = new List<ResourceType>();

			while (true)
			{
				ResourceType type = new ResourceType();
				exeFs.Read(DW, 0, 2);
				type.id = BitConverter.ToUInt16(DW, 0);
				if (type.id == 0)
					break;

				exeFs.Read(DW, 0, 2);
				type.count = BitConverter.ToUInt16(DW, 0);
				exeFs.Read(DD, 0, 4);
				type.reserved = BitConverter.ToUInt32(DD, 0);

				type.resources = new Resource[type.count];
				for (int i = 0; i < type.count; i++)
				{
					type.resources[i] = new Resource();
					exeFs.Read(DW, 0, 2);
					type.resources[i].dataOffset = BitConverter.ToUInt16(DW, 0);
					exeFs.Read(DW, 0, 2);
					type.resources[i].length = BitConverter.ToUInt16(DW, 0);
					exeFs.Read(DW, 0, 2);
					type.resources[i].flags = (ResourceFlags)BitConverter.ToUInt16(DW, 0);
					exeFs.Read(DW, 0, 2);
					type.resources[i].id = BitConverter.ToUInt16(DW, 0);
					exeFs.Read(DD, 0, 4);
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
					exeFs.Position = neStart + tableOff + table.types[t].id;
					len = (byte)exeFs.ReadByte();
					str = new byte[len];
					exeFs.Read(str, 0, len);
					table.types[t].name = Encoding.ASCII.GetString(str);
				}
				else
					table.types[t].name = Consts.IdToName(table.types[t].id);

				for (int r = 0; r < table.types[t].resources.Length; r++)
				{
					if ((table.types[t].resources[r].id & 0x8000) == 0)
					{
						byte len;
						byte[] str;
						exeFs.Position = neStart + tableOff + table.types[t].resources[r].id;
						len = (byte)exeFs.ReadByte();
						str = new byte[len];
						exeFs.Read(str, 0, len);
						table.types[t].resources[r].name = Encoding.ASCII.GetString(str);
					}
					else
						table.types[t].resources[r].name = string.Format("{0}", table.types[t].resources[r].id & 0x7FFF);

					table.types[t].resources[r].data = new byte[table.types[t].resources[r].length * (1 << table.alignment_shift)];
					exeFs.Position = table.types[t].resources[r].dataOffset * (1 << table.alignment_shift);
					exeFs.Read(table.types[t].resources[r].data, 0, table.types[t].resources[r].data.Length);
				}
			}

			exeFs.Position = oldPosition;

			return table;
		}
	}
}
