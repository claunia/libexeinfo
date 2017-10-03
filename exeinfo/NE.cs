using System;
using System.Runtime.InteropServices;

namespace exeinfo
{
    public static class NE
    {
        public const ushort Signature = 0x454E;

		//[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct Header
        {
            public ushort signature;
            public byte linker_major;
            public byte linker_minor;
            public ushort entry_table_offset;
            public ushort entry_table_length;
            public uint crc;
            public ProgramFlags program_flags;
            public ApplicationFlags application_flags;
            public byte auto_data_segment_index;
            public ushort initial_heap;
            public ushort initial_stack;
            public uint entry_point;
            public uint stack_pointer;
            public ushort segment_count;
            public ushort reference_count;
            public ushort nonresident_table_size;
            public ushort segment_table_offset;
            public ushort resource_table_offset;
            public ushort resident_names_offset;
            public ushort module_reference_offset;
            public ushort imported_names_offset;
            public uint nonresident_names_offset;
            public ushort movable_entries;
            public ushort alignment_shift;
            public ushort resource_entries;
            public TargetOS target_os;
            public OS2Flags os2_flags;
            public ushort return_thunks_offset;
            public ushort segment_reference_thunks;
            public ushort minimum_swap_area;
            public byte os_minor;
            public byte os_major;
        }

        [Flags]
        public enum ProgramFlags : byte
        {
            NoDGroup = 0,
            SingleDGroup = 1,
            MultipleDGroup = 2,
            GlobalInit = 1 << 2,
            ProtectedMode = 1 << 3,
            i86 = 1 << 4,
            i286 = 1 << 5,
            i386 = 1 << 6,
            i87 = 1 << 7
        }

        public enum TargetOS : byte
        {
            Unknown = 0,
            OS2 = 1,
            Windows = 2,
            DOS = 3,
            Win32 = 4,
            Borland = 5
        }

        [Flags]
        public enum ApplicationFlags : byte
        {
            FullScreen = 1,
            GUICompatible = 2,
            Errors = 1 << 5,
            NonConforming = 1 << 6,
            DLL = 1 << 7
        }

        [Flags]
        public enum OS2Flags : byte
        {
            LongFilename = 1 << 0,
            ProtectedMode2 = 1 << 1,
            ProportionalFonts = 1 << 2,
            GangloadArea = 1 << 3,
        }

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

            if(header.target_os == TargetOS.OS2)
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
            Console.WriteLine("\tFile alignment shift: {0}", 2 << header.alignment_shift);
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
    }
}
