//
// Structs.cs
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

using System.Runtime.InteropServices;

namespace libexeinfo.Os2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ResourceTableEntry
    {
        public ushort etype;
        public ushort ename;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MenuHeader
    {
        public ushort cb;
        public ushort type;
        public ushort cp;
        public ushort offset;
        public ushort count;
    }

    /// <summary>
    ///     Followed by data, if <see cref="MenuItem.style" /> is <see cref="MenuType.MIS_TEXT" /> a null-terminated
    ///     string follows, if <see cref="MenuType.MIS_BITMAP" /> a resource ID follows and if
    ///     <see cref="MenuType.MIS_SUBMENU" /> another header follows.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MenuItem
    {
        public MenuType      style;
        public MenuAttribute attrib;
        public ushort        id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DialogTemplateItem
    {
        public ushort fsItemStatus;
        public ushort cChildren;
        public ushort cchClassName;
        public ushort offClassName;
        public ushort cchText;
        public ushort offText;
        public uint   flStyle;
        public short  x;
        public short  y;
        public short  cx;
        public short  cy;
        public ushort id;
        public ushort offPresParams;
        public ushort offCtlData;
    }

    /// <summary>
    ///     Dialog template, followed by several <see cref="DialogTemplateItem" />
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DialogTemplate
    {
        public ushort cbTemplate;
        public ushort type;
        public ushort codepage;
        public ushort offadlgti;
        public ushort fsTemplateStatus;
        public ushort iItemFocus;
        public ushort coffPresParams;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BitmapArrayHeader
    {
        public ushort Type;
        public uint   Size;
        public uint   Next;
        public ushort XDisplay;
        public ushort YDisplay;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BitmapInfoHeader
    {
        public ushort Type;
        public uint   Size;
        public short  XHotspot;
        public short  YHostpot;
        public uint   Offset;
        public uint   Fix;
        public ushort X;
        public ushort Y;
        public ushort Planes;
        public ushort BitsPerPlane;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rgb
    {
        public byte Blue;
        public byte Green;
        public byte Red;
    }

    public class DecodedBitmap
    {
        public uint   BitsPerPixel;
        public uint   Height;
        public int[]  Pixels;
        public string Type;
        public uint   Width;
        public short  XHotspot;
        public short  YHostpot;
    }
}