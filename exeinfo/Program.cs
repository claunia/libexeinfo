//
// Program.cs
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
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace exeinfo
{
    class MainClass
    {
        static libexeinfo.MZ.Header mzHdr;
        static libexeinfo.NE.Header neHdr;

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

            byte[] buffer = new byte[Marshal.SizeOf(typeof(libexeinfo.MZ.Header))];

            exeFs.Read(buffer, 0, buffer.Length);
			IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
			mzHdr = (libexeinfo.MZ.Header)Marshal.PtrToStructure(hdrPtr, typeof(libexeinfo.MZ.Header));
			Marshal.FreeHGlobal(hdrPtr);

            if(mzHdr.signature == libexeinfo.MZ.Signature)
            {
                recognized = true;
                Console.Write(libexeinfo.MZ.GetInfo(mzHdr));

                if (mzHdr.new_offset < exeFs.Length)
                {
                    exeFs.Seek(mzHdr.new_offset, SeekOrigin.Begin);

                    buffer = new byte[Marshal.SizeOf(typeof(libexeinfo.NE.Header))];
					exeFs.Read(buffer, 0, buffer.Length);
					hdrPtr = Marshal.AllocHGlobal(buffer.Length);
					Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
					neHdr = (libexeinfo.NE.Header)Marshal.PtrToStructure(hdrPtr, typeof(libexeinfo.NE.Header));
					Marshal.FreeHGlobal(hdrPtr);

                    if (neHdr.signature == libexeinfo.NE.Signature)
                    {
                        Console.Write(libexeinfo.NE.GetInfo(neHdr));
                        libexeinfo.NE.ResourceTable resources = libexeinfo.NE.GetResources(exeFs, mzHdr.new_offset, neHdr.resource_table_offset);
                        foreach(libexeinfo.NE.ResourceType type in resources.types)
                        {
                            if((type.id & 0x7FFF) == (int)libexeinfo.NE.ResourceTypes.RT_VERSION)
                            {
                                foreach(libexeinfo.NE.Resource resource in type.resources)
                                {
                                    libexeinfo.NE.Version vers = new libexeinfo.NE.Version(resource.data);
                                    Console.WriteLine("\tVersion resource {0}:", resource.name);
                                    Console.WriteLine("\t\tFile version: {0}", vers.FileVersion);
                                    Console.WriteLine("\t\tProduct version: {0}", vers.ProductVersion);
                                    Console.WriteLine("\t\tFile type: {0}", libexeinfo.NE.Version.TypeToString(vers.FileType));
                                    if(vers.FileType == libexeinfo.NE.VersionFileType.VFT_DRV)
                                        Console.WriteLine("\t\tFile subtype: {0} driver", libexeinfo.NE.Version.DriverToString(vers.FileSubtype));
									else if (vers.FileType == libexeinfo.NE.VersionFileType.VFT_DRV)
                                        Console.WriteLine("\t\tFile subtype: {0} font", libexeinfo.NE.Version.FontToString(vers.FileSubtype));
									else if(vers.FileSubtype > 0)
										Console.WriteLine("\t\tFile subtype: {0}", (uint)vers.FileSubtype);
									Console.WriteLine("\t\tFile flags: {0}", vers.FileFlags);
                                    Console.WriteLine("\t\tFile OS: {0}", libexeinfo.NE.Version.OsToString(vers.FileOS));

                                    foreach (KeyValuePair<string, Dictionary<string, string>> strByLang in vers.StringsByLanguage)
                                    {
                                        string cultureName;
                                        string encodingName;

                                        try
                                        {
                                            cultureName = new CultureInfo(Convert.ToInt32(strByLang.Key.Substring(0, 4), 16)).DisplayName;
                                        }
                                        catch
                                        {
                                            cultureName = string.Format("unsupported culture 0x{0:X4}", Convert.ToInt32(strByLang.Key.Substring(0, 4), 16));
                                        }

										try
										{
                                            encodingName = Encoding.GetEncoding(Convert.ToInt32(strByLang.Key.Substring(4), 16)).EncodingName;
										}
										catch
										{
											encodingName = string.Format("unsupported encoding 0x{0:X4}", Convert.ToInt32(strByLang.Key.Substring(4), 16));
										}

                                        Console.WriteLine("\t\tStrings for {0} in codepage {1}:", cultureName, encodingName);
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
