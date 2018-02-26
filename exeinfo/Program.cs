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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using libexeinfo;

namespace exeinfo
{
    static class MainClass
    {
        public static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("exeinfo version 0.1 © 2017 Natalia Portillo");
                Console.WriteLine("Usage: exeinfo file.exe");
                return;
            }

            bool recognized = false;

            IExecutable mzExe   = new MZ(args[0]);
            IExecutable neExe   = new NE(args[0]);
            IExecutable stExe   = new AtariST(args[0]);
            IExecutable lxExe   = new LX(args[0]);
            IExecutable coffExe = new COFF(args[0]);
            IExecutable peExe   = new PE(args[0]);

            if(neExe.Recognized)
            {
                recognized = true;
                Console.Write(neExe.Information);
                if(((NE)neExe).Versions != null)
                    foreach(NE.Version vers in ((NE)neExe).Versions)
                    {
                        Console.WriteLine("\tVersion resource {0}:",  vers.Name);
                        Console.WriteLine("\t\tFile version: {0}",    vers.FileVersion);
                        Console.WriteLine("\t\tProduct version: {0}", vers.ProductVersion);
                        Console.WriteLine("\t\tFile type: {0}",       NE.Version.TypeToString(vers.FileType));
                        if(vers.FileType == NE.VersionFileType.VFT_DRV)
                            Console.WriteLine("\t\tFile subtype: {0} driver",
                                              NE.Version.DriverToString(vers.FileSubtype));
                        else if(vers.FileType == NE.VersionFileType.VFT_DRV)
                            Console.WriteLine("\t\tFile subtype: {0} font", NE.Version.FontToString(vers.FileSubtype));
                        else if(vers.FileSubtype > 0)
                            Console.WriteLine("\t\tFile subtype: {0}", (uint)vers.FileSubtype);
                        Console.WriteLine("\t\tFile flags: {0}",       vers.FileFlags);
                        Console.WriteLine("\t\tFile OS: {0}",          NE.Version.OsToString(vers.FileOS));

                        foreach(KeyValuePair<string, Dictionary<string, string>> strByLang in vers.StringsByLanguage)
                        {
                            string cultureName;
                            string encodingName;

                            try
                            {
                                cultureName = new CultureInfo(Convert.ToInt32(strByLang.Key.Substring(0, 4), 16))
                                   .DisplayName;
                            }
                            catch
                            {
                                cultureName =
                                    $"unsupported culture 0x{Convert.ToInt32(strByLang.Key.Substring(0, 4), 16):X4}";
                            }

                            try
                            {
                                encodingName = Encoding
                                              .GetEncoding(Convert.ToInt32(strByLang.Key.Substring(4), 16))
                                              .EncodingName;
                            }
                            catch
                            {
                                encodingName =
                                    $"unsupported encoding 0x{Convert.ToInt32(strByLang.Key.Substring(4), 16):X4}";
                            }

                            Console.WriteLine("\t\tStrings for {0} in codepage {1}:", cultureName, encodingName);
                            foreach(KeyValuePair<string, string> strings in strByLang.Value)
                                Console.WriteLine("\t\t\t{0}: {1}", strings.Key, strings.Value);
                        }
                    }

                if(((NE)neExe).ResidentNames != null)
                {
                    Console.WriteLine("\tResident names:");
                    foreach(NE.ResidentName name in ((NE)neExe).ResidentNames)
                        Console.WriteLine("\t\t{0} at index {1}", name.name, name.entryTableIndex);
                }

                if(((NE)neExe).NonResidentNames != null)
                {
                    Console.WriteLine("\tNon-resident names:");
                    foreach(NE.ResidentName name in ((NE)neExe).NonResidentNames)
                        Console.WriteLine("\t\t{0} at index {1}", name.name, name.entryTableIndex);
                }
            }
            else if(lxExe.Recognized)
            {
                recognized = true;
                Console.Write(lxExe.Information);
            }
            else if(peExe.Recognized)
            {
                recognized = true;
                Console.Write(peExe.Information);
            }
            else if(mzExe.Recognized)
            {
                recognized = true;
                Console.Write(mzExe.Information);
            }

            if(stExe.Recognized)
            {
                recognized = true;
                Console.Write(stExe.Information);
                if(((AtariST)stExe).resourceStream     != null ||
                   (((AtariST)stExe).Resource.rsh_vrsn != 0 && ((AtariST)stExe).Resource.rsh_vrsn != 1 &&
                    ((AtariST)stExe).Resource.rsh_vrsn != 4 && ((AtariST)stExe).Resource.rsh_vrsn != 5))
                {
                    Console.WriteLine("\tResources:");
                    Console.WriteLine("\t\t{0} OBJECTs start at {1}", ((AtariST)stExe).Resource.rsh_nobs, ((AtariST)stExe).Resource.rsh_object);
                    Console.WriteLine("\t\t{0} TEDINFOs start at {1}", ((AtariST)stExe).Resource.rsh_nted, ((AtariST)stExe).Resource.rsh_tedinfo);
                    Console.WriteLine("\t\t{0} ICONBLKs start at {1}", ((AtariST)stExe).Resource.rsh_nib, ((AtariST)stExe).Resource.rsh_iconblk);
                    Console.WriteLine("\t\t{0} BITBLKs start at {1}", ((AtariST)stExe).Resource.rsh_nbb, ((AtariST)stExe).Resource.rsh_bitblk);
                    Console.WriteLine("\t\t{0} object trees start at {1}", ((AtariST)stExe).Resource.rsh_ntree, ((AtariST)stExe).Resource.rsh_trindex);
                    Console.WriteLine("\t\t{0} free strings start at {1}", ((AtariST)stExe).Resource.rsh_nstring, ((AtariST)stExe).Resource.rsh_frstr);
                    Console.WriteLine("\t\t{0} free images start at {1}", ((AtariST)stExe).Resource.rsh_nimages, ((AtariST)stExe).Resource.rsh_frimg);
                    Console.WriteLine("\t\tString data starts at {0}", ((AtariST)stExe).Resource.rsh_string);
                    Console.WriteLine("\t\tImage data starts at {0}", ((AtariST)stExe).Resource.rsh_imdata);
                    Console.WriteLine("\t\tStandard resource data is {0} bytes", ((AtariST)stExe).Resource.rsh_rssize);
                }
            }

            if(coffExe.Recognized)
            {
                recognized = true;
                Console.Write(coffExe.Information);
            }

            if(!recognized) Console.WriteLine("Executable format not recognized");
        }
    }
}