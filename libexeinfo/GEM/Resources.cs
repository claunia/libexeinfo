//
// Resources.cs
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace libexeinfo
{
    public static partial class GEM
    {
        internal static TreeObjectNode ProcessResourceObject(IList<ObjectNode> nodes, ref List<short> knownNodes,
                                                             short             nodeNumber, Stream     resourceStream,
                                                             List<string>      strings, bool          bigEndian,
                                                             Encoding          encoding)
        {
            TreeObjectNode node = new TreeObjectNode
            {
                type   = (ObjectTypes)(nodes[nodeNumber].ob_type & 0xff),
                flags  = (ObjectFlags)nodes[nodeNumber].ob_flags,
                state  = (ObjectStates)nodes[nodeNumber].ob_state,
                data   = nodes[nodeNumber].ob_spec,
                x      = nodes[nodeNumber].ob_x,
                y      = nodes[nodeNumber].ob_y,
                width  = nodes[nodeNumber].ob_width,
                height = nodes[nodeNumber].ob_height
            };

            byte[] buffer;
            switch(node.type)
            {
                case ObjectTypes.G_TEXT:
                case ObjectTypes.G_BOXTEXT:
                case ObjectTypes.G_FTEXT:
                case ObjectTypes.G_FBOXTEXT:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    buffer                  = new byte[Marshal.SizeOf(typeof(TedInfo))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    TedInfo ted = bigEndian
                                      ? BigEndianMarshal.ByteArrayToStructureBigEndian<TedInfo>(buffer)
                                      : BigEndianMarshal.ByteArrayToStructureLittleEndian<TedInfo>(buffer);

                    node.TedInfo = new TextBlock
                    {
                        Font          = (ObjectFont)ted.te_font,
                        Justification = (ObjectJustification)ted.te_just,
                        BorderColor   = (ObjectColors)((ted.te_color      & BorderColorMask) >> 12),
                        TextColor     = (ObjectColors)((ted.te_color      & TextColorMask)   >> 8),
                        Transparency  = (ted.te_color                     & TransparentColor) != TransparentColor,
                        Fill          = (ObjectFillPattern)((ted.te_color & FillPatternMask) >> 4),
                        InsideColor   = (ObjectColors)(ted.te_color       & InsideColorMask),
                        Thickness     = ted.te_thickness
                    };

                    byte[] tmpStr;

                    if(ted.te_ptext > 0 && ted.te_ptext < resourceStream.Length && ted.te_txtlen > 0)
                    {
                        tmpStr                  = new byte[ted.te_txtlen];
                        resourceStream.Position = ted.te_ptext;
                        resourceStream.Read(tmpStr, 0, ted.te_txtlen);
                        node.TedInfo.Text = StringHandlers.CToString(tmpStr, encoding);
                        if(!string.IsNullOrWhiteSpace(node.TedInfo.Text)) strings.Add(node.TedInfo.Text.Trim());
                    }

                    if(ted.te_pvalid > 0 && ted.te_pvalid < resourceStream.Length && ted.te_txtlen > 0)
                    {
                        tmpStr                  = new byte[ted.te_txtlen];
                        resourceStream.Position = ted.te_pvalid;
                        resourceStream.Read(tmpStr, 0, ted.te_txtlen);
                        node.TedInfo.Validation = StringHandlers.CToString(tmpStr, encoding);
                        if(!string.IsNullOrWhiteSpace(node.TedInfo.Validation))
                            strings.Add(node.TedInfo.Validation.Trim());
                    }

                    if(ted.te_ptmplt > 0 && ted.te_ptmplt < resourceStream.Length && ted.te_tmplen > 0)
                    {
                        tmpStr                  = new byte[ted.te_tmplen];
                        resourceStream.Position = ted.te_ptmplt;
                        resourceStream.Read(tmpStr, 0, ted.te_tmplen);
                        node.TedInfo.Template = StringHandlers.CToString(tmpStr, encoding);
                        if(!string.IsNullOrWhiteSpace(node.TedInfo.Template)) strings.Add(node.TedInfo.Template.Trim());
                    }

                    break;
                case ObjectTypes.G_IMAGE:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    buffer                  = new byte[Marshal.SizeOf(typeof(BitBlock))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    BitBlock bitBlock = bigEndian
                                            ? BigEndianMarshal.ByteArrayToStructureBigEndian<BitBlock>(buffer)
                                            : BigEndianMarshal.ByteArrayToStructureLittleEndian<BitBlock>(buffer);

                    node.BitBlock = new BitmapBlock
                    {
                        Color  = (ObjectColors)bitBlock.bi_color,
                        Height = bitBlock.bi_hl,
                        Width  = bitBlock.bi_wb * 8,
                        X      = bitBlock.bi_x,
                        Y      = bitBlock.bi_y
                    };

                    if(bitBlock.bi_pdata == 0 || bitBlock.bi_pdata >= resourceStream.Length) break;

                    node.BitBlock.Data      = new byte[bitBlock.bi_wb * bitBlock.bi_hl];
                    resourceStream.Position = bitBlock.bi_pdata;
                    resourceStream.Read(node.BitBlock.Data, 0, node.BitBlock.Data.Length);

                    // Because the image is stored as words, they get reversed on PC GEM (Little-endian)
                    if(!bigEndian)
                    {
                        byte[] data = new byte[node.BitBlock.Data.Length];
                        for(int i = 0; i < data.Length; i += 2)
                        {
                            data[i] = node.BitBlock.Data[i + 1];
                            data[i                         + 1] = node.BitBlock.Data[i];
                        }

                        node.BitBlock.Data = data;
                    }

                    break;
                case ObjectTypes.G_USERDEF:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    buffer                  = new byte[Marshal.SizeOf(typeof(ApplicationBlock))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    node.ApplicationBlock = bigEndian
                                                ? BigEndianMarshal
                                                   .ByteArrayToStructureBigEndian<ApplicationBlock>(buffer)
                                                : BigEndianMarshal
                                                   .ByteArrayToStructureLittleEndian<ApplicationBlock>(buffer);
                    break;
                case ObjectTypes.G_ICON:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    buffer                  = new byte[Marshal.SizeOf(typeof(IconBlock))];
                    resourceStream.Read(buffer, 0, buffer.Length);

                    node.IconBlock = GetIconBlock(resourceStream, buffer, bigEndian, encoding);

                    if(!string.IsNullOrWhiteSpace(node.IconBlock.Text)) strings.Add(node.IconBlock.Text.Trim());

                    break;
                case ObjectTypes.G_CICON:
                    // Do nothing, it is done separately...
                    break;
                case ObjectTypes.G_BUTTON:
                case ObjectTypes.G_STRING:
                case ObjectTypes.G_TITLE:
                case ObjectTypes.G_SHORTCUT:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    List<byte> chars        = new List<byte>();
                    while(true)
                    {
                        int character = resourceStream.ReadByte();

                        if(character <= 0) break;

                        chars.Add((byte)character);
                    }

                    node.String = StringHandlers.CToString(chars.ToArray(), encoding);
                    if(!string.IsNullOrWhiteSpace(node.String)) strings.Add(node.String.Trim());
                    break;
            }

            knownNodes.Add(nodeNumber);

            if(nodes[nodeNumber].ob_head > 0 && !knownNodes.Contains(nodes[nodeNumber].ob_head))
                node.child = ProcessResourceObject(nodes,   ref knownNodes, nodes[nodeNumber].ob_head, resourceStream,
                                                   strings, bigEndian,      encoding);

            if(nodes[nodeNumber].ob_next > 0 && !knownNodes.Contains(nodes[nodeNumber].ob_next))
                node.sibling = ProcessResourceObject(nodes,   ref knownNodes, nodes[nodeNumber].ob_next, resourceStream,
                                                     strings, bigEndian,      encoding);

            return node;
        }

        static Icon GetIconBlock(Stream resourceStream, byte[] buffer, bool bigEndian, Encoding encoding)
        {
            long oldPosition = resourceStream.Position;

            IconBlock iconBlock = bigEndian
                                      ? BigEndianMarshal.ByteArrayToStructureBigEndian<IconBlock>(buffer)
                                      : BigEndianMarshal.ByteArrayToStructureLittleEndian<IconBlock>(buffer);

            Icon icon = new Icon
            {
                Width           = iconBlock.ib_wicon,
                Height          = iconBlock.ib_hicon,
                X               = iconBlock.ib_xicon,
                Y               = iconBlock.ib_yicon,
                ForegroundColor = (ObjectColors)((iconBlock.ib_char >> 12)           & 0x000F),
                BackgroundColor = (ObjectColors)((iconBlock.ib_char >> 8)            & 0x000F),
                Character       = encoding.GetString(new[] {(byte)(iconBlock.ib_char & 0xFF)})[0],
                CharX           = iconBlock.ib_xchar,
                CharY           = iconBlock.ib_ychar,
                TextX           = iconBlock.ib_xtext,
                TextY           = iconBlock.ib_ytext,
                TextWidth       = iconBlock.ib_wtext,
                TextHeight      = iconBlock.ib_htext
            };

            if(iconBlock.ib_ptext > 0 && iconBlock.ib_ptext < resourceStream.Length)
            {
                resourceStream.Position = iconBlock.ib_ptext;
                List<byte> chars        = new List<byte>();
                while(true)
                {
                    int character = resourceStream.ReadByte();

                    if(character <= 0) break;

                    chars.Add((byte)character);
                }

                icon.Text = StringHandlers.CToString(chars.ToArray(), encoding);
            }

            if(iconBlock.ib_pdata > 0 && iconBlock.ib_pdata < resourceStream.Length)
            {
                resourceStream.Position = iconBlock.ib_pdata;
                icon.Data               = new byte[icon.Width * icon.Height / 8];
                resourceStream.Read(icon.Data, 0, icon.Data.Length);

                // Because the image is stored as words, they get reversed on PC GEM (Little-endian)
                if(!bigEndian)
                {
                    byte[] data = new byte[icon.Data.Length];
                    for(int i = 0; i < data.Length; i += 2)
                    {
                        data[i] = icon.Data[i + 1];
                        data[i                + 1] = icon.Data[i];
                    }

                    icon.Data = data;
                }
            }

            if(iconBlock.ib_pmask > 0 && iconBlock.ib_pmask < resourceStream.Length)
            {
                resourceStream.Position = iconBlock.ib_pmask;
                icon.Mask               = new byte[icon.Width * icon.Height / 8];
                resourceStream.Read(icon.Mask, 0, icon.Mask.Length);

                // Because the mask is stored as words, they get reversed on PC GEM (Little-endian)
                if(!bigEndian)
                {
                    byte[] mask = new byte[icon.Mask.Length];
                    for(int i = 0; i < mask.Length; i += 2)
                    {
                        mask[i] = icon.Mask[i + 1];
                        mask[i                + 1] = icon.Mask[i];
                    }

                    icon.Mask = mask;
                }
            }

            resourceStream.Position = oldPosition;
            return icon;
        }

        public static ColorIcon[] GetColorIcons(Stream resourceStream, int colorIc, bool bigEndian, Encoding encoding)
        {
            byte[] buffer;

            if(colorIc == -1 || colorIc >= resourceStream.Length) return null;

            resourceStream.Position = colorIc;

            int cicons = 0;

            while(true)
            {
                buffer = new byte[4];
                resourceStream.Read(buffer, 0, buffer.Length);

                if(BitConverter.ToInt32(buffer, 0) == -1) break;

                cicons++;
            }

            ColorIcon[] colorIcons = new ColorIcon[cicons];

            for(int i = 0; i < cicons; i++)
            {
                buffer = new byte[Marshal.SizeOf(typeof(IconBlock))];
                resourceStream.Read(buffer, 0, buffer.Length);
                IconBlock iconBlock = BigEndianMarshal.ByteArrayToStructureBigEndian<IconBlock>(buffer);
                int       isize     = iconBlock.ib_wicon * iconBlock.ib_hicon / 8;

                buffer                  =  new byte[4];
                resourceStream.Position -= 2;
                resourceStream.Read(buffer, 0, buffer.Length);
                int numRez = BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);

                colorIcons[i] = new ColorIcon
                {
                    Color      = new ColorIconPlane[numRez],
                    Monochrome = new Icon
                    {
                        Width           = iconBlock.ib_wicon,
                        Height          = iconBlock.ib_hicon,
                        X               = iconBlock.ib_xicon,
                        Y               = iconBlock.ib_yicon,
                        ForegroundColor = (ObjectColors)((iconBlock.ib_char >> 12)           & 0x000F),
                        BackgroundColor = (ObjectColors)((iconBlock.ib_char >> 8)            & 0x000F),
                        Character       = encoding.GetString(new[] {(byte)(iconBlock.ib_char & 0xFF)})[0],
                        CharX           = iconBlock.ib_xchar,
                        CharY           = iconBlock.ib_ychar,
                        TextX           = iconBlock.ib_xtext,
                        TextY           = iconBlock.ib_ytext,
                        TextWidth       = iconBlock.ib_wtext,
                        TextHeight      = iconBlock.ib_htext,
                        Data            = new byte[isize],
                        Mask            = new byte[isize]
                    }
                };

                resourceStream.Read(colorIcons[i].Monochrome.Data, 0, isize);

                // Because the image is stored as words, they get reversed on PC GEM (Little-endian)
                if(!bigEndian)
                {
                    byte[] data = new byte[colorIcons[i].Monochrome.Data.Length];
                    for(int d = 0; d < data.Length; d += 2)
                    {
                        data[d] = colorIcons[d].Monochrome.Data[d + 1];
                        data[d                                    + 1] = colorIcons[d].Monochrome.Data[d];
                    }

                    colorIcons[i].Monochrome.Data = data;
                }

                resourceStream.Read(colorIcons[i].Monochrome.Mask, 0, isize);

                // Because the mask is stored as words, they get reversed on PC GEM (Little-endian)
                if(!bigEndian)
                {
                    byte[] mask = new byte[colorIcons[i].Monochrome.Mask.Length];
                    for(int m = 0; m < mask.Length; m += 2)
                    {
                        mask[m] = colorIcons[m].Monochrome.Mask[m + 1];
                        mask[m                                    + 1] = colorIcons[m].Monochrome.Mask[m];
                    }

                    colorIcons[i].Monochrome.Mask = mask;
                }

                if(iconBlock.ib_ptext > 0 && iconBlock.ib_ptext < resourceStream.Length)
                {
                    long oldPosition        = resourceStream.Position;
                    resourceStream.Position = iconBlock.ib_ptext;
                    List<byte> chars        = new List<byte>();
                    while(true)
                    {
                        int character = resourceStream.ReadByte();

                        if(character <= 0) break;

                        chars.Add((byte)character);
                    }

                    colorIcons[i].Monochrome.Text = StringHandlers.CToString(chars.ToArray(), encoding);
                    resourceStream.Position       = oldPosition + 12;
                }
                else
                {
                    byte[] ptext = new byte[12];
                    resourceStream.Read(ptext, 0, 12);
                    colorIcons[i].Monochrome.Text = StringHandlers.CToString(ptext, encoding);
                }

                colorIcons[i].Color = new ColorIconPlane[numRez];

                for(int r = 0; r < numRez; r++)
                {
                    byte[] data;
                    byte[] mask;

                    buffer = new byte[Marshal.SizeOf(typeof(ColorIconBlock))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    ColorIconBlock cib = BigEndianMarshal.ByteArrayToStructureBigEndian<ColorIconBlock>(buffer);

                    colorIcons[i].Color[r] = new ColorIconPlane
                    {
                        Planes = cib.num_planes,
                        Data   = new byte[isize * cib.num_planes],
                        Mask   = new byte[isize]
                    };

                    resourceStream.Read(colorIcons[i].Color[r].Data, 0, isize * cib.num_planes);

                    // Because the image is stored as words, they get reversed on PC GEM (Little-endian)
                    if(!bigEndian)
                    {
                        data = new byte[colorIcons[i].Color[r].Data.Length];
                        for(int d = 0; d < data.Length; d += 2)
                        {
                            data[d] = colorIcons[d].Color[r].Data[d + 1];
                            data[d                                  + 1] = colorIcons[d].Color[r].Data[d];
                        }

                        colorIcons[i].Color[r].Data = data;
                    }

                    resourceStream.Read(colorIcons[i].Color[r].Mask, 0, isize);

                    // Because the mask is stored as words, they get reversed on PC GEM (Little-endian)
                    if(!bigEndian)
                    {
                        mask = new byte[colorIcons[i].Color[r].Mask.Length];
                        for(int m = 0; m < mask.Length; m += 2)
                        {
                            mask[m] = colorIcons[m].Color[r].Mask[m + 1];
                            mask[m                                  + 1] = colorIcons[m].Color[r].Mask[m];
                        }

                        colorIcons[i].Color[r].Mask = mask;
                    }

                    if(cib.sel_data == 0) continue;

                    colorIcons[i].Color[r].SelectedData = new byte[isize * cib.num_planes];
                    colorIcons[i].Color[r].SelectedMask = new byte[isize];

                    resourceStream.Read(colorIcons[i].Color[r].SelectedData, 0, isize * cib.num_planes);

                    // Because the image is stored as words, they get reversed on PC GEM (Little-endian)
                    if(!bigEndian)
                    {
                        data = new byte[colorIcons[i].Color[r].SelectedData.Length];
                        for(int d = 0; d < data.Length; d += 2)
                        {
                            data[d] = colorIcons[d].Color[r].SelectedData[d + 1];
                            data[d                                          + 1] =
                                colorIcons[d].Color[r].SelectedData[d];
                        }

                        colorIcons[i].Color[r].SelectedData = data;
                    }

                    resourceStream.Read(colorIcons[i].Color[r].SelectedMask, 0, isize);

                    // Because the mask is stored as words, they get reversed on PC GEM (Little-endian)
                    if(bigEndian) continue;

                    mask = new byte[colorIcons[i].Color[r].SelectedMask.Length];
                    for(int m = 0; m < mask.Length; m += 2)
                    {
                        mask[m] = colorIcons[m].Color[r].SelectedMask[m + 1];
                        mask[m                                          + 1] = colorIcons[m].Color[r].SelectedMask[m];
                    }

                    colorIcons[i].Color[r].SelectedMask = mask;
                }
            }

            return colorIcons;
        }

        public static MagiCResourceHeader GemToMagiC(GemResourceHeader header)
        {
            return new MagiCResourceHeader
            {
                rsh_vrsn    = header.rsh_vrsn,
                rsh_object  = header.rsh_object,
                rsh_tedinfo = header.rsh_tedinfo,
                rsh_iconblk = header.rsh_iconblk,
                rsh_bitblk  = header.rsh_bitblk,
                rsh_frstr   = header.rsh_frstr,
                rsh_string  = header.rsh_string,
                rsh_imdata  = header.rsh_imdata,
                rsh_frimg   = header.rsh_frimg,
                rsh_trindex = header.rsh_trindex,
                rsh_nobs    = header.rsh_nobs,
                rsh_ntree   = header.rsh_ntree,
                rsh_nted    = header.rsh_nted,
                rsh_nib     = header.rsh_nib,
                rsh_nbb     = header.rsh_nbb,
                rsh_nstring = header.rsh_nstring,
                rsh_nimages = header.rsh_nimages,
                rsh_rssize  = header.rsh_rssize
            };
        }
    }
}