//
// Program.cs
//
// Author:
//       Natalia Portillo <claunia@claunia.com>
//
// Copyright (c) 2017-2018 Copyright © Claunia.com
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
using System.Linq;
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
                Console.WriteLine("exeinfo version 0.1 © 2017-2018 Natalia Portillo");
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
            IExecutable geosExe = new Geos(args[0]);

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
                        Console.WriteLine("\t\tFile flags: {0}", vers.FileFlags);
                        Console.WriteLine("\t\tFile OS: {0}",    NE.Version.OsToString(vers.FileOS));

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

                if(neExe.Strings != null && neExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in neExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }
            else if(lxExe.Recognized)
            {
                recognized = true;
                Console.Write(lxExe.Information);

                if(lxExe.Strings != null && lxExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in lxExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }
            else if(peExe.Recognized)
            {
                recognized = true;
                Console.Write(peExe.Information);

                if(peExe.Strings != null && peExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in peExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }
            else if(mzExe.Recognized)
            {
                recognized = true;
                Console.Write(mzExe.Information);
                if(((MZ)mzExe).ResourceStream != null || ((MZ)mzExe).ResourceHeader.rsh_vrsn != 0 &&
                   ((MZ)mzExe).ResourceHeader.rsh_vrsn                                       != 1 &&
                   ((MZ)mzExe).ResourceHeader.rsh_vrsn                                       != 4 &&
                   ((MZ)mzExe).ResourceHeader.rsh_vrsn                                       != 5)
                    PrintGemResources(((MZ)mzExe).ResourceHeader,    ((MZ)mzExe).ResourceObjectRoots,
                                      ((MZ)mzExe).ResourceExtension, ((MZ)mzExe).GemColorIcons);

                if(mzExe.Strings != null && mzExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in mzExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }

            if(stExe.Recognized)
            {
                recognized = true;
                Console.Write(stExe.Information);
                if(((AtariST)stExe).ResourceStream != null || ((AtariST)stExe).ResourceHeader.rsh_vrsn != 0 &&
                   ((AtariST)stExe).ResourceHeader.rsh_vrsn                                            != 1 &&
                   ((AtariST)stExe).ResourceHeader.rsh_vrsn                                            != 4 &&
                   ((AtariST)stExe).ResourceHeader.rsh_vrsn                                            != 5)
                    PrintGemResources(((AtariST)stExe).ResourceHeader,    ((AtariST)stExe).ResourceObjectRoots,
                                      ((AtariST)stExe).ResourceExtension, ((AtariST)stExe).GemColorIcons);

                if(stExe.Strings != null && stExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in stExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }

            if(coffExe.Recognized)
            {
                recognized = true;
                Console.Write(coffExe.Information);

                if(coffExe.Strings != null && coffExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in coffExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }

            if(geosExe.Recognized)
            {
                recognized = true;
                Console.Write(geosExe.Information);

                if(geosExe.Strings != null && geosExe.Strings.Any())
                {
                    Console.WriteLine("\tStrings:");
                    foreach(string str in geosExe.Strings) Console.WriteLine("\t\t{0}", str);
                }
            }

            if(!recognized) Console.WriteLine("Executable format not recognized");
        }

        static void PrintGemResources(GEM.MagiCResourceHeader           resourceHeader,
                                      IReadOnlyList<GEM.TreeObjectNode> roots,
                                      GEM.GemResourceExtension          resourceExtension,
                                      GEM.ColorIcon[]                   colorIcons)
        {
            Console.WriteLine("\t\tGEM Resources:");
            Console.WriteLine("\t\t\t{0} OBJECTs start at {1}",  resourceHeader.rsh_nobs, resourceHeader.rsh_object);
            Console.WriteLine("\t\t\t{0} TEDINFOs start at {1}", resourceHeader.rsh_nted, resourceHeader.rsh_tedinfo);
            Console.WriteLine("\t\t\t{0} ICONBLKs start at {1}", resourceHeader.rsh_nib,  resourceHeader.rsh_iconblk);
            Console.WriteLine("\t\t\t{0} BITBLKs start at {1}",  resourceHeader.rsh_nbb,  resourceHeader.rsh_bitblk);
            Console.WriteLine("\t\t\t{0} object trees start at {1}", resourceHeader.rsh_ntree,
                              resourceHeader.rsh_trindex);
            Console.WriteLine("\t\t\t{0} free strings start at {1}", resourceHeader.rsh_nstring,
                              resourceHeader.rsh_frstr);
            Console.WriteLine("\t\t\t{0} free images start at {1}", resourceHeader.rsh_nimages,
                              resourceHeader.rsh_frimg);
            Console.WriteLine("\t\t\tString data starts at {0}",           resourceHeader.rsh_string);
            Console.WriteLine("\t\t\tImage data starts at {0}",            resourceHeader.rsh_imdata);
            Console.WriteLine("\t\t\tStandard resource data is {0} bytes", resourceHeader.rsh_rssize);
            if(resourceHeader.rsh_vrsn >= 4)
            {
                Console.WriteLine("\t\t\tColor icon table starts at {0}", resourceExtension.color_ic);
                Console.WriteLine("\t\t\tThere are {0}more extensions",
                                  resourceExtension.end_extensions == 0 ? "no " : "");
                Console.WriteLine("\t\t\tExtended resource data is {0} bytes", resourceExtension.filesize);
            }

            if(roots == null || roots.Count <= 0) return;

            for(int i = 0; i < roots.Count; i++)
            {
                Console.WriteLine("\t\t\tObject tree {0}:", i);
                PrintGemResourceTree(roots[i], 4, colorIcons);
            }
        }

        static void PrintGemResourceTree(GEM.TreeObjectNode node, int level, GEM.ColorIcon[] colorIcons)
        {
            for(int i = 0; i < level; i++) Console.Write("\t");

            string thickStr;

            switch(node.type)
            {
                case GEM.ObjectTypes.G_BOX:
                case GEM.ObjectTypes.G_IBOX:
                    Console.WriteLine("{0} ({1} {2}) {3} border, {4} text, {5} interior, {6} fill, {7} mode," + " coordinates ({8},{9}) size {10}x{11}",
                                      node.type, node.flags, node.state,
                                      (GEM.ObjectColors)((node.data & 0xFFFF      & GEM.BorderColorMask) >> 12),
                                      (GEM.ObjectColors)((node.data & 0xFFFF      & GEM.TextColorMask)   >> 8),
                                      (GEM.ObjectColors)((node.data & 0xFFFF      & GEM.InsideColorMask) >> 8),
                                      (GEM.ObjectFillPattern)((node.data & 0xFFFF & GEM.FillPatternMask) >> 4),
                                      (node.data & 0xFFFF & GEM.TransparentColor) != 0 ? "transparent" : "replace",
                                      node.x, node.y, node.width, node.height);
                    break;
                case GEM.ObjectTypes.G_BOXCHAR:
                    sbyte thickness = (sbyte)((node.data & 0xFF0000) >> 16);

                    if(thickness      < 0) thickStr = $"{thickness * -1} pixels outward thickness";
                    else if(thickness > 0) thickStr = $"{thickness} pixels inward thickness";
                    else thickStr                   = "no thickness";

                    char character =
                        Claunia.Encoding.Encoding.AtariSTEncoding.GetString(new[]
                        {
                            (byte)((node.data & 0xFF000000) >> 24)
                        })[0];

                    Console.WriteLine(
                                      "{0} ({1} {2}) {3} border, {4} text, {5} interior, {6} fill, {7} mode, {8}," +
                                      " '{9}' character, coordinates ({10},{11}) size {12}x{13}", node.type, node.flags,
                                      node.state, (GEM.ObjectColors)((node.data & 0xFFFF & GEM.BorderColorMask) >> 12),
                                      (GEM.ObjectColors)((node.data & 0xFFFF             & GEM.TextColorMask)   >> 8),
                                      (GEM.ObjectColors)((node.data & 0xFFFF             & GEM.InsideColorMask) >> 8),
                                      (GEM.ObjectFillPattern)((node.data & 0xFFFF        & GEM.FillPatternMask) >> 4),
                                      (node.data & 0xFFFF & GEM.TransparentColor) != 0 ? "transparent" : "replace",
                                      thickStr, character, node.x, node.y, node.width, node.height);
                    break;
                case GEM.ObjectTypes.G_BUTTON:
                case GEM.ObjectTypes.G_STRING:
                case GEM.ObjectTypes.G_TITLE:
                    Console.WriteLine("{0} ({1} {2}), coordinates ({3},{4}) size {5}x{6}: {7}", node.type, node.flags,
                                      node.state, node.x, node.y, node.width, node.height, node.String);
                    break;
                case GEM.ObjectTypes.G_TEXT:
                case GEM.ObjectTypes.G_BOXTEXT:
                case GEM.ObjectTypes.G_FTEXT:
                case GEM.ObjectTypes.G_FBOXTEXT:
                    if(node.TedInfo == null) goto default;

                    if(node.TedInfo.Thickness < 0)
                        thickStr                                 = $"{node.TedInfo.Thickness * -1} pixels outward thickness";
                    else if(node.TedInfo.Thickness > 0) thickStr = $"{node.TedInfo.Thickness} pixels inward thickness";
                    else thickStr                                = "no thickness";

                    Console.WriteLine("{0} ({1} {2}), coordinates ({3},{4}) size {5}x{6}, font {7}, {8}-justified," + " {9}, {10} border, {11} text, {12} interior, {13} fill, {14} mode," + " text: \"{15}\", validation: \"{16}\", template: \"{17}\"",
                                      node.type, node.flags, node.state, node.x, node.y, node.width, node.height,
                                      node.TedInfo.Font, node.TedInfo.Justification, thickStr, node.TedInfo.BorderColor,
                                      node.TedInfo.TextColor, node.TedInfo.InsideColor, node.TedInfo.Fill,
                                      node.TedInfo.Transparency ? "transparent" : "replace", node.TedInfo.Text,
                                      node.TedInfo.Validation, node.TedInfo.Template);
                    break;
                case GEM.ObjectTypes.G_IMAGE:
                    if(node.BitBlock == null) goto default;

                    Console.WriteLine("{0} ({1} {2}), coordinates ({3},{4}) size {5}x{6}, colored {7}, {8} bytes",
                                      node.type, node.flags, node.state, node.BitBlock.X, node.BitBlock.Y,
                                      node.BitBlock.Width, node.BitBlock.Height, node.BitBlock.Color,
                                      node.BitBlock.Data?.Length);
                    break;
                /*
            case GEM.ObjectTypes.G_USERDEF: break;*/
                case GEM.ObjectTypes.G_ICON:
                    if(node.IconBlock == null) goto default;

                    Console.WriteLine(
                                      "{0} ({1} {2}), coordinates ({3},{4}) size {5}x{6}, {7} foreground,"            +
                                      " {8} background, char '{9}' at ({10},{11}), {12} bytes data, text \"{13}\" at" +
                                      " ({14},{15}) within a box {16}x{17} pixels", node.type, node.flags, node.state,
                                      node.IconBlock.X, node.IconBlock.Y, node.IconBlock.Width, node.IconBlock.Height,
                                      node.IconBlock.ForegroundColor, node.IconBlock.BackgroundColor,
                                      node.IconBlock.Character, node.IconBlock.CharX, node.IconBlock.CharY,
                                      node.IconBlock.Data?.Length, node.IconBlock.Text, node.IconBlock.TextX,
                                      node.IconBlock.TextY, node.IconBlock.TextWidth, node.IconBlock.TextHeight);

                    break;
                case GEM.ObjectTypes.G_CICON:
                    if(colorIcons                       == null || colorIcons.Length < node.data ||
                       colorIcons[node.data]            == null ||
                       colorIcons[node.data].Monochrome == null)
                    {
                        Console.WriteLine("{0} ({1} {2}) with index {3} NOT FOUND", node.type, node.flags, node.state,
                                          node.data);
                        break;
                    }

                    Console.WriteLine(
                                      "{0} ({1} {2}), coordinates ({3},{4}) size {5}x{6}, {7} foreground,"            +
                                      " {8} background, char '{9}' at ({10},{11}), {12} bytes data, text \"{13}\" at" +
                                      " ({14},{15}) within a box {16}x{17} pixels, with {18} different planes",
                                      node.type, node.flags, node.state, colorIcons[node.data].Monochrome.X,
                                      colorIcons[node.data].Monochrome.Y, colorIcons[node.data].Monochrome.Width,
                                      colorIcons[node.data].Monochrome.Height,
                                      colorIcons[node.data].Monochrome.ForegroundColor,
                                      colorIcons[node.data].Monochrome.BackgroundColor,
                                      colorIcons[node.data].Monochrome.Character,
                                      colorIcons[node.data].Monochrome.CharX, colorIcons[node.data].Monochrome.CharY,
                                      colorIcons[node.data].Monochrome.Data?.Length,
                                      colorIcons[node.data].Monochrome.Text, colorIcons[node.data].Monochrome.TextX,
                                      colorIcons[node.data].Monochrome.TextY,
                                      colorIcons[node.data].Monochrome.TextWidth,
                                      colorIcons[node.data].Monochrome.TextHeight, colorIcons[node.data].Color.Length);
                    break;
                default:
                    Console.WriteLine("{0} ({1} {2}) data = {3}, coordinates ({4},{5}) size {6}x{7}", node.type,
                                      node.flags, node.state, node.data, node.x, node.y, node.width, node.height);
                    break;
            }

            if(node.child != null) PrintGemResourceTree(node.child, level + 1, colorIcons);

            if(node.sibling != null) PrintGemResourceTree(node.sibling, level, colorIcons);
        }
    }
}