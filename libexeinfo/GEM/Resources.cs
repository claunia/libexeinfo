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

using System.Collections.Generic;
using System.IO;
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
                type   = (ObjectTypes)nodes[nodeNumber].ob_type,
                flags  = (ObjectFlags)nodes[nodeNumber].ob_flags,
                state  = (ObjectStates)nodes[nodeNumber].ob_state,
                data   = nodes[nodeNumber].ob_spec,
                x      = nodes[nodeNumber].ob_x,
                y      = nodes[nodeNumber].ob_y,
                width  = nodes[nodeNumber].ob_width,
                height = nodes[nodeNumber].ob_height
            };

            byte[]     buffer;
            List<byte> chars;
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

                    if(ted.te_ptext > 0 && ted.te_ptext < resourceStream.Length && ted.te_txtlen > 1)
                    {
                        tmpStr                  = new byte[ted.te_txtlen - 1];
                        resourceStream.Position = ted.te_ptext;
                        resourceStream.Read(tmpStr, 0, ted.te_txtlen - 1);
                        node.TedInfo.Text = encoding.GetString(tmpStr);
                        strings.Add(node.TedInfo.Text.Trim());
                    }

                    if(ted.te_pvalid > 0 && ted.te_pvalid < resourceStream.Length && ted.te_txtlen > 1)
                    {
                        tmpStr                  = new byte[ted.te_txtlen - 1];
                        resourceStream.Position = ted.te_pvalid;
                        resourceStream.Read(tmpStr, 0, ted.te_txtlen - 1);
                        node.TedInfo.Validation = encoding.GetString(tmpStr);
                        strings.Add(node.TedInfo.Validation.Trim());
                    }

                    if(ted.te_ptmplt > 0 && ted.te_ptmplt < resourceStream.Length && ted.te_tmplen > 1)
                    {
                        tmpStr                  = new byte[ted.te_tmplen - 1];
                        resourceStream.Position = ted.te_ptmplt;
                        resourceStream.Read(tmpStr, 0, ted.te_tmplen - 1);
                        node.TedInfo.Template = encoding.GetString(tmpStr);
                        strings.Add(node.TedInfo.Template.Trim());
                    }

                    break;
                // TODO: This is indeed a CUT from a bigger image, need to cut it out
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

                    break;
                case ObjectTypes.G_USERDEF:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    buffer                  = new byte[Marshal.SizeOf(typeof(UserBlock))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    node.UserBlock = bigEndian
                                         ? BigEndianMarshal.ByteArrayToStructureBigEndian<UserBlock>(buffer)
                                         : BigEndianMarshal.ByteArrayToStructureLittleEndian<UserBlock>(buffer);
                    break;
                case ObjectTypes.G_ICON:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    buffer                  = new byte[Marshal.SizeOf(typeof(IconBlock))];
                    resourceStream.Read(buffer, 0, buffer.Length);
                    IconBlock iconBlock = bigEndian
                                              ? BigEndianMarshal.ByteArrayToStructureBigEndian<IconBlock>(buffer)
                                              : BigEndianMarshal.ByteArrayToStructureLittleEndian<IconBlock>(buffer);

                    node.IconBlock = new Icon
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
                        chars                   = new List<byte>();
                        while(true)
                        {
                            int character = resourceStream.ReadByte();

                            if(character <= 0) break;

                            chars.Add((byte)character);
                        }

                        node.IconBlock.Text = encoding.GetString(chars.ToArray());
                        strings.Add(node.IconBlock.Text.Trim());
                    }

                    if(iconBlock.ib_pdata > 0 && iconBlock.ib_pdata < resourceStream.Length)
                    {
                        resourceStream.Position = iconBlock.ib_pdata;
                        node.IconBlock.Data     = new byte[node.IconBlock.Width * node.IconBlock.Height / 8];
                        resourceStream.Read(node.IconBlock.Data, 0, node.IconBlock.Data.Length);
                    }

                    if(iconBlock.ib_pmask > 0 && iconBlock.ib_pmask < resourceStream.Length)
                    {
                        resourceStream.Position = iconBlock.ib_pmask;
                        node.IconBlock.Mask     = new byte[node.IconBlock.Width * node.IconBlock.Height / 8];
                        resourceStream.Read(node.IconBlock.Mask, 0, node.IconBlock.Mask.Length);
                    }

                    break;
                case ObjectTypes.G_CICON:
                    //Console.WriteLine("ColorIconBlock pointer {0}", node.data);
                    break;
                case ObjectTypes.G_BUTTON:
                case ObjectTypes.G_STRING:
                case ObjectTypes.G_TITLE:
                    if(node.data <= 0 || node.data >= resourceStream.Length) break;

                    resourceStream.Position = node.data;
                    chars                   = new List<byte>();
                    while(true)
                    {
                        int character = resourceStream.ReadByte();

                        if(character <= 0) break;

                        chars.Add((byte)character);
                    }

                    node.String = encoding.GetString(chars.ToArray());
                    strings.Add(node.String.Trim());
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
    }
}