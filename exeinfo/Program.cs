using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace exeinfo
{
    class MainClass
    {
        static MZ.Header mzHdr;
        static NE.Header neHdr;

        public static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("exeinfo version 0.1 © 2017 Natalia Portillo");
                Console.WriteLine("Usage: exeinfo file.exe");
                return;
            }

            FileStream exeFs = File.Open(args[0], FileMode.Open, FileAccess.Read);

            bool recognized = false;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(MZ.Header))];

            exeFs.Read(buffer, 0, buffer.Length);
			IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
			mzHdr = (MZ.Header)Marshal.PtrToStructure(hdrPtr, typeof(MZ.Header));
			Marshal.FreeHGlobal(hdrPtr);

            if(mzHdr.signature == MZ.Consts.Signature)
            {
                recognized = true;
                MZ.Info.PrintInfo(mzHdr);

                if (mzHdr.new_offset < exeFs.Length)
                {
                    exeFs.Seek(mzHdr.new_offset, SeekOrigin.Begin);

                    buffer = new byte[Marshal.SizeOf(typeof(NE.Header))];
					exeFs.Read(buffer, 0, buffer.Length);
					hdrPtr = Marshal.AllocHGlobal(buffer.Length);
					Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
					neHdr = (NE.Header)Marshal.PtrToStructure(hdrPtr, typeof(NE.Header));
					Marshal.FreeHGlobal(hdrPtr);

                    if (neHdr.signature == NE.Consts.Signature)
                    {
                        NE.Info.PrintInfo(neHdr);
                        NE.ResourceTable resources = NE.Info.GetResources(exeFs, mzHdr.new_offset, neHdr.resource_table_offset);
                        foreach(NE.ResourceType type in resources.types)
                        {
                            if((type.id & 0x7FFF) == (int)NE.ResourceTypes.RT_VERSION)
                            {
                                foreach(NE.Resource resource in type.resources)
                                {
                                    NE.Version vers = new NE.Version(resource.data);
                                    Console.WriteLine("\tVersion resource {0}:", resource.name);
                                    Console.WriteLine("\t\tFile version: {0}", vers.FileVersion);
                                    Console.WriteLine("\t\tProduct version: {0}", vers.ProductVersion);
                                    Console.WriteLine("\t\tFile type: {0}", NE.Version.TypeToString(vers.FileType));
                                    if(vers.FileType == NE.VersionFileType.VFT_DRV)
                                        Console.WriteLine("\t\tFile subtype: {0} driver", NE.Version.DriverToString(vers.FileSubtype));
									else if (vers.FileType == NE.VersionFileType.VFT_DRV)
                                        Console.WriteLine("\t\tFile subtype: {0} font", NE.Version.FontToString(vers.FileSubtype));
									else if(vers.FileSubtype > 0)
										Console.WriteLine("\t\tFile subtype: {0}", (uint)vers.FileSubtype);
									Console.WriteLine("\t\tFile flags: {0}", vers.FileFlags);
                                    Console.WriteLine("\t\tFile OS: {0}", NE.Version.OsToString(vers.FileOS));

                                    foreach (KeyValuePair<string, Dictionary<string, string>> strByLang in vers.StringsByLanguage)
                                    {
                                        CultureInfo cult = new CultureInfo(Convert.ToInt32(strByLang.Key.Substring(0, 4), 16));
                                        Encoding encoding = Encoding.GetEncoding(Convert.ToInt32(strByLang.Key.Substring(4), 16));
                                        Console.WriteLine("\t\tStrings for {0} in codepage {1}:", cult.DisplayName, encoding.EncodingName);
                                        foreach(KeyValuePair<string, string> strings in strByLang.Value)
                                            Console.WriteLine("\t\t\t{0}: {1}", strings.Key, strings.Value);
                                    }
                                }
                            }
                        }
                    }
				}
            }

            if (!recognized)
                Console.WriteLine("Executalbe format not recognized");
		}
    }
}
