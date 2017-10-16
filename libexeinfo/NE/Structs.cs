using System;
using System.Runtime.InteropServices;

namespace libexeinfo.NE
{
	[StructLayout(LayoutKind.Sequential/*, Pack = 2*/)]
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

	public struct ResourceTable
	{
		public ushort alignment_shift;
		public ResourceType[] types;
	}

	public struct ResourceType
	{
		public ushort id;
		public ushort count;
		public uint reserved;
		public Resource[] resources;

		// Not sequentially stored
		public string name;
	}

	public struct Resource
	{
		public ushort dataOffset;
		public ushort length;
		public ResourceFlags flags;
		public ushort id;
		public uint reserved;

		// Not sequentially stored
		public string name;
		public byte[] data;
	}
	
    class VersionNode
	{
		public ushort cbNode;
		public ushort cbData;
		public string szName;
		public byte[] rgbData;
		public VersionNode[] children;
	}

    [StructLayout(LayoutKind.Sequential)]
    public class FixedFileInfo
    {
		public uint dwSignature;
		public uint dwStrucVersion;
		public uint dwFileVersionMS;
		public uint dwFileVersionLS;
		public uint dwProductVersionMS;
		public uint dwProductVersionLS;
		public uint dwFileFlagsMask;
		public uint dwFileFlags;
		public uint dwFileOS;
		public uint dwFileType;
		public uint dwFileSubtype;
		public uint dwFileDateMS;
		public uint dwFileDateLS;
	}
}
