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

namespace libexeinfo
{
    public partial class AtariST
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AtariHeader
        {
            public ushort signature;
            public int   text_len;
            public int   data_len;
            public int   bss_len;
            public int   symb_len;
            public uint   mint;
            public uint   flags;
            public ushort absflags;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AtariResourceHeader
        {
            /// <summary>
            ///     Contains the version number of the resource file. This value is 0x0000 or 0x0001 in old format RSC files and has
            ///     the third bit set (i.e. 0x0004) in the new file format. If file is in the new format, it is appended at the end with <see cref="AtariResourceExtension"/>
            /// </summary>
            public short rsh_vrsn;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the OBJECT structures.
            /// </summary>
            public short rsh_object;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the TEDINFO structures.
            /// </summary>
            public short rsh_tedinfo;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the ICONBLK structures.
            /// </summary>
            public short rsh_iconblk;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the BITBLK structures.
            /// </summary>
            public short rsh_bitblk;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the string pointer table.
            /// </summary>
            public short rsh_frstr;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the string data.
            /// </summary>
            public short rsh_string;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the image data.
            /// </summary>
            public short rsh_imdata;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the image pointer table.
            /// </summary>
            public short rsh_frimg;
            /// <summary>
            ///     Contains an offset from the beginning of the file to the tree pointer table.
            /// </summary>
            public short rsh_trindex;
            /// <summary>
            ///     Number of OBJECTs in the file.
            /// </summary>
            public short rsh_nobs;
            /// <summary>
            ///     Number of object trees in the file.
            /// </summary>
            public short rsh_ntree;
            /// <summary>
            ///     Number of TEDINFOs in the file.
            /// </summary>
            public short rsh_nted;
            /// <summary>
            ///     Number of ICONBLKs in the file.
            /// </summary>
            public short rsh_nib;
            /// <summary>
            ///     Number of BITBLKs in the file.
            /// </summary>
            public short rsh_nbb;
            /// <summary>
            ///     Number of free strings in the file.
            /// </summary>
            public short rsh_nstring;
            /// <summary>
            ///     Number of free images in the file.
            /// </summary>
            public short rsh_nimages;
            /// <summary>
            ///     Size of the resource file (in bytes). Note that this is the size of the old format resource file. If the newer
            ///     format file is being used then this value can be used as an offset to the extension array.
            /// </summary>
            public short rsh_rssize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AtariResourceExtension
        {
            /// <summary>
            /// Size of the file
            /// </summary>
            public int filesize;
            /// <summary>
            /// Slot for color icons containing an offset to <see cref="ColorIconBlock"/> table. The table is an array of <see cref="int"/> offsets in file with -1 meaning table end. 
            /// </summary>
            public int color_ic;
            /// <summary>
            /// If not 0, it's an unknown extension, 0 means last extension
            /// </summary>
            public int end_extensions;
        }

        /// <summary>
        /// The OBJECT structure contains values that describe the object, its relationship to the other objects in the tree, and its location relative to its parent or (in the case of the root object) the screen.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ObjectNode
        {
            /// <summary>
            /// A word containing the index of the object's next sibling in the object tree array
            /// </summary>
            public short ob_next;
            /// <summary>
            /// A word containing the index of the first child: the head of the list of the object's children in the object tree array
            /// </summary>
            public short ob_head;
            /// <summary>
            /// A word contianing the index of the last child: the tail of the list of the object's children in the object tree array
            /// </summary>
            public short ob_tail;
            /// <summary>
            /// A word containing the object type. GEM AES ignored the high byte of this word
            /// </summary>
            public short ob_type;
            /// <summary>
            /// A word containing the object flags
            /// </summary>
            public ushort ob_flags;
            /// <summary>
            /// A word containing the object state
            /// </summary>
            public ushort ob_state;
            /// <summary>
            /// A long value containing object specific data. Depending on the object's type, can be a pointer to any combination of word and/or byte values that add up to 32 bits.
            /// </summary>
            public int ob_spec;
            /// <summary>
            /// A word containing the X-coordinate of the object relative to its parent or (for the root object) the screen
            /// </summary>
            public short ob_x;
            /// <summary>
            /// A word containing the Y-coordinate of the object relative to its parent or (for the root object) the screen
            /// </summary>
            public short ob_y;
            /// <summary>
            /// A word containing the width of the object in pixels
            /// </summary>
            public short ob_width;
            /// <summary>
            /// A word containing the height of the object in pixels
            /// </summary>
            public short ob_height;
        }

        /// <summary>
        /// The  TEDINFO structure lets a user edit formatted text. The object types G_TEXT, G_BOXTEXT, G_FTEXT and G_FBOXTEXT use their <see cref="ObjectNode.ob_spec"/> to point to TEDINFO structures.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TedInfo
        {
            /// <summary>
            /// A pointer to the actual text. If the first character is '@', the field is blank. 
            /// </summary>
            public int te_ptext;
            /// <summary>
            /// A pointer to a string template for any data entry. The editable portion is represented by underscores.
            /// </summary>
            public int te_ptmplt;
            /// <summary>
            /// A pointer to a text string contianing characters tht validate any entered text
            /// </summary>
            public int te_pvalid;
            /// <summary>
            /// A word identifying the font used to draw the text. 3 for system font, 5 for small font.
            /// </summary>
            public short te_font;
            /// <summary>
            /// Reserved for future use
            /// </summary>
            public short te_resvd1;
            /// <summary>
            /// A word identifying the type of text justification desired. 0 = left, 1 = right, 2 = center
            /// </summary>
            public short te_just;
            /// <summary>
            /// A word identifying the color and pattern of box-type objects
            /// </summary>
            public short te_color;
            /// <summary>
            /// Reserved for future use
            /// </summary>
            public short te_resvd2;
            /// <summary>
            /// A word containing the thickness in pixels of the border of the text box. 0 for none, positive for inside, negative for outside
            /// </summary>
            public short te_thickness;
            /// <summary>
            /// A word containing the length of the string pointed by <see cref="te_ptext"/>.
            /// </summary>
            public short te_txtlen;
            /// <summary>
            /// A word containing the length of the string pointed by <see cref="te_ptmplt"/>.
            /// </summary>
            public short te_tmplen;
        }

        /// <summary>
        /// The ICONBLK structure is used to hold the data that defines icons. The object type G_ICON points with its <see cref="ObjectNode.ob_spec"/> pointer to an ICONBLK structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IconBlock
        {
            /// <summary>
            /// A pointer to an array of words representing the mask bit image of the icon
            /// </summary>
            public int ib_pmask;
            /// <summary>
            /// A pointer to an array of words representing the data bit image of the icon
            /// </summary>
            public int ib_pdata;
            /// <summary>
            /// A pointer to the icon's text
            /// </summary>
            public int ib_ptext;
            /// <summary>
            /// A word containing a character to be drawn in the icon. The high byte contains the foreground color in the high nibble and the background color in the low nibble.
            /// </summary>
            public short ib_char;
            /// <summary>
            /// A word containing the X-coordinate of <see cref="ib_char"/>
            /// </summary>
            public short ib_xchar;
            /// <summary>
            /// A word containing the Y-coordinate of <see cref="ib_char"/>
            /// </summary>
            public short ib_ychar;
            /// <summary>
            /// A word containing the X-coordinate of the icon
            /// </summary>
            public short ib_xicon;
            /// <summary>
            /// A word containing the Y-coordinate of the icon
            /// </summary>
            public short ib_yicon;
            /// <summary>
            /// A word containing the width of the icon in pixels. Must be divisible by 16.
            /// </summary>
            public short ib_wicon;
            /// <summary>
            /// A word containing the height of the icon in pixels
            /// </summary>
            public short ib_hicon;
            /// <summary>
            /// A word containing the X-coordinate of the icon's text
            /// </summary>
            public short ib_xtext;
            /// <summary>
            /// A word containing the Y-coordinate of the icon's text
            /// </summary>
            public short ib_ytext;
            /// <summary>
            /// A word containing the width of a rectangle in which the icon's text will be centered
            /// </summary>
            public short ib_wtext;
            /// <summary>
            /// A word containing the height of the icon's text in pixels
            /// </summary>
            public short ib_htext;
            /// <summary>
            /// Zeros
            /// </summary>
            public short empty;
        }

        /// <summary>
        /// The object type G_IMAGE uses the BITBLK structure to draw bit images like cursor forms or icons
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BitBlock
        {
            /// <summary>
            /// A pointer to an array of words contianing the bit image
            /// </summary>
            public int bi_pdata;
            /// <summary>
            /// A word containing the width of the <see cref="bi_pdata"/> array in bytes
            /// </summary>
            public short bi_wb;
            /// <summary>
            /// A word containing the height of the bit block in scan lines (pixels)
            /// </summary>
            public short bi_hl;
            /// <summary>
            /// A word containing the source X in bit form, relative to the <see cref="bi_pdata"/> array
            /// </summary>
            public short bi_x;
            /// <summary>
            /// A word containing the source Y in bit form, relative to the <see cref="bi_pdata"/> array
            /// </summary>
            public short bi_y;
            /// <summary>
            /// A word containing the color GEM AES uses when displaying the bit image.
            /// </summary>
            public short bi_color;
            /// <summary>
            /// Zeros
            /// </summary>
            public short empty;
        }

        /// <summary>
        /// The USERBLK structure is used to locate and call an application-defined routine that will draw and/or change an object. The object type G_UERDEF points with its <see cref="ObjectNode.ob_spec"/> pointer to an USERBLK structure. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UserBlock
        {
            /// <summary>
            /// A pointer to the routine for drawing and/or changing the object
            /// </summary>
            public int ub_code;
            /// <summary>
            /// A long value (optionally provided by the application) passed as a parameter when calling the routine
            /// </summary>
            public int ub_parm;
        }

        /// <summary>
        /// The PARMBLK structure is used to store information relevant to the application's drawing or changing an object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ParameterBlock
        {
            /// <summary>
            /// A pointer to the object tree that contains the application defined object
            /// </summary>
            public int pb_tree;
            /// <summary>
            /// A word containing the object index of the application defined object
            /// </summary>
            public short pb_obj;
            /// <summary>
            /// A word containing the old state of an object to be changed
            /// </summary>
            public short pb_prevstate;
            /// <summary>
            /// A word containing the changed (new) state of an object
            /// </summary>
            public short pb_currstate;
            /// <summary>
            /// A word containing the X-coordinate of a rectangle defining the location of the object on the physical screen
            /// </summary>
            public short pb_x;
            /// <summary>
            /// A word containing the Y-coordinate of a rectangle defining the location of the object on the physical screen
            /// </summary>
            public short pb_y;
            /// <summary>
            /// A word containing the width in pixels of a rectanble defining the size of the object on the physical screen
            /// </summary>
            public short pb_w;
            /// <summary>
            /// A word containing the height in pixels of a rectanble defining the size of the object on the physical screen
            /// </summary>
            public short pb_h;
            /// <summary>
            /// A word containing the X-coordinate of the current clip rectangle on the physical screen
            /// </summary>
            public short pb_xc;
            /// <summary>
            /// A word containing the Y-coordinate of the current clip rectangle on the physical screen
            /// </summary>
            public short pb_yc;
            /// <summary>
            /// A word containing the width in pixels of the current clip rectnagle on the physical screen
            /// </summary>
            public short pb_wc;
            /// <summary>
            /// A word containing the heigth in pixels of the current clip rectnagle on the physical screen
            /// </summary>
            public short pb_hc;
            /// <summary>
            /// A long value, identical to <see cref="UserBlock.ub_parm"/>, that is passed to the application when it is time for the application to draw or change the object. Low word.
            /// </summary>
            public short pb_parm_low;
            /// <summary>
            /// A long value, identical to <see cref="UserBlock.ub_parm"/>, that is passed to the application when it is time for the application to draw or change the object. High word.
            /// </summary>
            public short pb_parm_high;
            /// <summary>
            /// Zeros
            /// </summary>
            public short empty;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ColorIcon
        {
            /// <summary>
            /// Number of planes in the following data
            /// </summary>
            public short num_planes;
            /// <summary>
            /// Pointer to color bitmap in standard form
            /// </summary>
            public int col_data;
            /// <summary>
            /// Pointer to single plane mask of <see cref="col_data"/>
            /// </summary>
            public int col_mask;
            /// <summary>
            /// Pointer to color bitmap of selected icon
            /// </summary>
            public int sel_data;
            /// <summary>
            /// Pointer to single plane mask of <see cref="sel_data"/>
            /// </summary>
            public int sel_mask;
            /// <summary>
            /// Pointer to next icon
            /// </summary>
            public int next_res;
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ColorIconBlock
        {
            /// <summary>
            /// Default monochrome icon
            /// </summary>
            IconBlock monoblk;
            /// <summary>
            /// List of color icons for diferent resolutions
            /// </summary>
            ColorIcon[] mainlist;
        }
    }
}