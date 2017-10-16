using System.Runtime.InteropServices;

namespace libexeinfo.MZ
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Header
	{
		public ushort signature;
		public ushort bytes_in_last_block;
		public ushort blocks_in_file;
		public ushort num_relocs;
		public ushort header_paragraphs;
		public ushort min_extra_paragraphs;
		public ushort max_extra_paragraphs;
		public ushort ss;
		public ushort sp;
		public ushort checksum;
		public ushort ip;
		public ushort cs;
		public ushort reloc_table_offset;
		public ushort overlay_number;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public ushort[] reserved;
		public ushort oem_id;
		public ushort oem_info;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public ushort[] reserved2;
		public uint new_offset;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct RelocationTableEntry
	{
		public ushort offset;
		public ushort segment;
	}
}
