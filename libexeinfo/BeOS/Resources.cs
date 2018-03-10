using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace libexeinfo.BeOS
{
    public static class Resources
    {
        public static ResourceTypeBlock[] Decode(byte[] data, bool bigEndian = false)
        {
            return bigEndian ? ResourceEdoced(data) : ResourceDecode(data);
        }

        static ResourceTypeBlock[] ResourceDecode(byte[] data)
        {
            uint pos = 0;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(ResourcesHeader))];
            Array.Copy(data, pos, buffer, 0, buffer.Length);
            ResourcesHeader header = BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourcesHeader>(buffer);

            if(header.magic != Consts.RESOURCES_HEADER_MAGIC) return null;

            pos    = header.index_section_offset;
            buffer = new byte[Marshal.SizeOf(typeof(ResourceIndexSectionHeader))];
            if(pos + buffer.Length > data.Length) return null;

            Array.Copy(data, pos, buffer, 0, buffer.Length);
            ResourceIndexSectionHeader indexHeader =
                BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceIndexSectionHeader>(buffer);
            pos += (uint)buffer.Length;

            ResourceIndexEntry[] indexes =
                new ResourceIndexEntry[(indexHeader.index_section_size -
                                        Marshal.SizeOf(typeof(ResourceIndexSectionHeader))) /
                                       Marshal.SizeOf(typeof(ResourceIndexEntry))];
            for(int i = 0; i < indexes.Length; i++)
            {
                buffer = new byte[Marshal.SizeOf(typeof(ResourceIndexEntry))];
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                indexes[i] =  BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceIndexEntry>(buffer);
                pos        += (uint)buffer.Length;
            }

            pos = indexHeader.info_table_offset;
            string[]       types       = new string[header.resource_count];
            bool           terminated  = true;
            string         currentType = null;
            string[]       names       = new string[header.resource_count];
            ResourceInfo[] infos       = new ResourceInfo[header.resource_count];

            for(int i = 0; i < header.resource_count; i++)
            {
                if(terminated)
                {
                    buffer = new byte[4];
                    Array.Copy(data, pos, buffer, 0, 4);
                    currentType =  Encoding.ASCII.GetString(buffer.Reverse().ToArray());
                    terminated  =  false;
                    pos         += 4;
                }

                buffer = new byte[Marshal.SizeOf(typeof(ResourceInfo))];
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                infos[i] =  BigEndianMarshal.ByteArrayToStructureLittleEndian<ResourceInfo>(buffer);
                pos      += (uint)buffer.Length;
                buffer   =  new byte[infos[i].name_size - 1];
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                names[i] =  Encoding.ASCII.GetString(buffer);
                pos      += (uint)(buffer.Length + 1);
                types[i] =  currentType;

                if(BitConverter.ToInt32(data, (int)pos)     != -1 ||
                   BitConverter.ToInt32(data, (int)pos + 4) != -1) continue;

                terminated =  true;
                pos        += 8;
            }

            Dictionary<string, List<Resource>> rezzes = new Dictionary<string, List<Resource>>();
            for(int i = 0; i < header.resource_count; i++)
            {
                rezzes.TryGetValue(types[i], out List<Resource> thisRezzes);

                if(thisRezzes == null) thisRezzes = new List<Resource>();

                Resource rez = new Resource
                {
                    id    = infos[i].id,
                    index = infos[i].index,
                    name  = names[i],
                    data  = new byte[indexes[infos[i].index - 1].size]
                };

                Array.Copy(data, indexes[infos[i].index - 1].offset, rez.data, 0, rez.data.Length);

                thisRezzes.Add(rez);
                rezzes.Remove(types[i]);
                rezzes.Add(types[i], thisRezzes);
            }

            List<ResourceTypeBlock> result = new List<ResourceTypeBlock>();
            foreach(KeyValuePair<string, List<Resource>> kvp in rezzes)
            {
                ResourceTypeBlock block = new ResourceTypeBlock {type = kvp.Key, resources = kvp.Value.ToArray()};
                result.Add(block);
            }

            return result.ToArray();
        }

        static ResourceTypeBlock[] ResourceEdoced(byte[] data)
        {
            uint pos = 0;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(ResourcesHeader))];
            Array.Copy(data, pos, buffer, 0, buffer.Length);
            ResourcesHeader header = BigEndianMarshal.ByteArrayToStructureBigEndian<ResourcesHeader>(buffer);

            if(header.magic != Consts.RESOURCES_HEADER_MAGIC) return null;

            pos    = header.index_section_offset;
            buffer = new byte[Marshal.SizeOf(typeof(ResourceIndexSectionHeader))];
            if(pos + buffer.Length > data.Length) return null;

            Array.Copy(data, pos, buffer, 0, buffer.Length);
            ResourceIndexSectionHeader indexHeader =
                BigEndianMarshal.ByteArrayToStructureBigEndian<ResourceIndexSectionHeader>(buffer);
            pos += (uint)buffer.Length;

            ResourceIndexEntry[] indexes =
                new ResourceIndexEntry[(indexHeader.index_section_size -
                                        Marshal.SizeOf(typeof(ResourceIndexSectionHeader))) /
                                       Marshal.SizeOf(typeof(ResourceIndexEntry))];
            for(int i = 0; i < indexes.Length; i++)
            {
                buffer = new byte[Marshal.SizeOf(typeof(ResourceIndexEntry))];
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                indexes[i] =  BigEndianMarshal.ByteArrayToStructureBigEndian<ResourceIndexEntry>(buffer);
                pos        += (uint)buffer.Length;
            }

            pos = indexHeader.info_table_offset;
            string[]       types       = new string[header.resource_count];
            bool           terminated  = true;
            string         currentType = null;
            string[]       names       = new string[header.resource_count];
            ResourceInfo[] infos       = new ResourceInfo[header.resource_count];

            for(int i = 0; i < header.resource_count; i++)
            {
                if(terminated)
                {
                    buffer = new byte[4];
                    Array.Copy(data, pos, buffer, 0, 4);
                    currentType =  Encoding.ASCII.GetString(buffer.ToArray());
                    terminated  =  false;
                    pos         += 4;
                }

                buffer = new byte[Marshal.SizeOf(typeof(ResourceInfo))];
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                infos[i] =  BigEndianMarshal.ByteArrayToStructureBigEndian<ResourceInfo>(buffer);
                pos      += (uint)buffer.Length;
                buffer   =  new byte[infos[i].name_size - 1];
                Array.Copy(data, pos, buffer, 0, buffer.Length);
                names[i] =  Encoding.ASCII.GetString(buffer);
                pos      += (uint)(buffer.Length + 1);
                types[i] =  currentType;

                if(BitConverter.ToInt32(data, (int)pos)     != -1 ||
                   BitConverter.ToInt32(data, (int)pos + 4) != -1) continue;

                terminated =  true;
                pos        += 8;
            }

            Dictionary<string, List<Resource>> rezzes = new Dictionary<string, List<Resource>>();
            for(int i = 0; i < header.resource_count; i++)
            {
                rezzes.TryGetValue(types[i], out List<Resource> thisRezzes);

                if(thisRezzes == null) thisRezzes = new List<Resource>();

                Resource rez = new Resource
                {
                    id    = infos[i].id,
                    index = infos[i].index,
                    name  = names[i],
                    data  = new byte[indexes[infos[i].index - 1].size]
                };

                Array.Copy(data, indexes[infos[i].index - 1].offset, rez.data, 0, rez.data.Length);

                thisRezzes.Add(rez);
                rezzes.Remove(types[i]);
                rezzes.Add(types[i], thisRezzes);
            }

            List<ResourceTypeBlock> result = new List<ResourceTypeBlock>();
            foreach(KeyValuePair<string, List<Resource>> kvp in rezzes)
            {
                ResourceTypeBlock block = new ResourceTypeBlock {type = kvp.Key, resources = kvp.Value.ToArray()};
                result.Add(block);
            }

            return result.ToArray();
        }
    }
}