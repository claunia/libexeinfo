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

            FileStream exeFs = File.Open(args[0], FileMode.Open, FileAccess.Read);

            bool recognized = false;

            IExecutable mzExe   = new MZ(exeFs);
            IExecutable neExe   = new NE(exeFs);
            IExecutable stExe   = new AtariST(exeFs);
            IExecutable lxExe   = new LX(exeFs);
            IExecutable coffExe = new COFF(exeFs);
            IExecutable peExe   = new PE(exeFs);

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