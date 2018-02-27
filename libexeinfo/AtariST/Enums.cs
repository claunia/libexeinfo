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

using System;

namespace libexeinfo
{
    public partial class AtariST
    {
        public enum ObjectTypes : short
        {
            /// <summary>
            ///     A graphic box. Its <see cref="ObjectNode.ob_spec" /> contains the object's color word and thickness.
            /// </summary>
            G_BOX = 20,
            /// <summary>
            ///     Graphic text. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a <see cref="TedInfo" /> structure in which the
            ///     value of <see cref="TedInfo.te_ptext" /> points to the actual text string as displayed.
            /// </summary>
            G_TEXT = 21,
            /// <summary>
            ///     A graphic box containing graphic text. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a
            ///     <see cref="TedInfo" /> structure in which the value of <see cref="TedInfo.te_ptext" /> points to the actual text
            ///     string as displayed.
            /// </summary>
            G_BOXTEXT = 22,
            /// <summary>
            ///     A graphic bit image. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a <see cref="BitBlock" /> structure.
            /// </summary>
            G_IMAGE = 23,
            /// <summary>
            ///     A programmed defined object. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a <see cref="UserBlock" />
            ///     structure.
            /// </summary>
            G_USERDEF = 24,
            /// <summary>
            ///     An invisible graphic box. Its <see cref="ObjectNode.ob_spec" /> contains the object's color word and thickness. It
            ///     has no fill pattern and no internal color. If its border has no thickness, it is truly invisible. If it does, it is
            ///     an outline.
            /// </summary>
            G_IBOX = 25,
            /// <summary>
            ///     A graphic text object centered in a box. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a null-terminated
            ///     text string.
            /// </summary>
            G_BUTTON = 26,
            /// <summary>
            ///     A graphic box containing a single text character. Its <see cref="ObjectNode.ob_spec" /> contains the character,
            ///     plus the object's color word and thickness.
            /// </summary>
            G_BOXCHAR = 27,
            /// <summary>
            ///     A graphic text object. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a null terminated string.
            /// </summary>
            G_STRING = 28,
            /// <summary>
            ///     Formatted graphic text. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a <see cref="TedInfo" /> structure in
            ///     which the value of <see cref="TedInfo.te_ptext" /> points to a text string that is merged with the template pointed
            ///     by the <see cref="TedInfo.te_ptmplt" /> before it is displayed.
            /// </summary>
            G_FTEXT = 29,
            /// <summary>
            ///     A graphic box containing formatted graphic text. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a
            ///     <see cref="TedInfo" /> structure in which the value of <see cref="TedInfo.te_ptext" /> points to a text string that
            ///     is merged with the template pointed by the <see cref="TedInfo.te_ptmplt" /> before it is displayed.
            /// </summary>
            G_FBOXTEXT = 30,
            /// <summary>
            ///     An object that describes an icon. Its <see cref="ObjectNode.ob_spec" /> is a pointer to an <see cref="IconBlock" />
            ///     structure.
            /// </summary>
            G_ICON = 31,
            /// <summary>
            ///     A graphic text string used in menu titles. Its <see cref="ObjectNode.ob_spec" /> value is a pointer to a null
            ///     terminated text string.
            /// </summary>
            G_TITLE = 32,
            /// <summary>
            ///     An object that describes a color icon. Its <see cref="ObjectNode.ob_spec" /> is a pointer to an <see cref="ColorIconBlock" />
            ///     structure.
            /// </summary>
            G_CICON = 31,
        }

        [Flags]
        public enum ObjectFlags : short
        {
            /// <summary>
            /// No flags
            /// </summary>
            None = 0x0000,
            /// <summary>
            /// Indicates that the user can select the object
            /// </summary>
            Selectable = 0x0001,
            /// <summary>
            /// Indicates that the Form Library will examine the object if the user enters a carriage return. No more than one object in a form can be flagged so.
            /// </summary>
            Default = 0x0002,
            /// <summary>
            /// Indicates that the Form Library will return control to the caller after the exit condition is satisfied. The condition is satisfied when the user clicks on the object.
            /// </summary>
            Exit = 0x0004,
            /// <summary>
            /// Indicates that an object is editable by the user in some way.
            /// </summary>
            Editable = 0x0008,
            /// <summary>
            /// An object called a radio button.
            /// </summary>
            Rbutton = 0x0010,
            /// <summary>
            /// Indicates that an object is the last in the tree
            /// </summary>
            Lastob = 0x0020,
            /// <summary>
            /// Inidicates that the Form Library will return control to the caller afeter the exit condition is satisfied. The condition is satisfied when the user clicks while the pointer is over the object.
            /// </summary>
            Touchexit = 0x0040,
            /// <summary>
            /// Makes a subtree invisible. 
            /// </summary>
            Hidetree = 0x0080,
            /// <summary>
            /// Indicates that the value in <see cref="ObjectNode.ob_spec"/> is a pointer to the actual value.
            /// </summary>
            Indirect = 0x0100
        }

        [Flags]
        public enum ObjectStates : short
        {
            /// <summary>
            /// Indicates that the object is drawn in normal colors
            /// </summary>
            Normal = 0x0000,
            /// <summary>
            /// Indicates that the object is highlighted by being drawn with reversed colors
            /// </summary>
            Selected = 0x0001,
            /// <summary>
            /// Indicates that an 'X' is drawn in the object.
            /// </summary>
            Crossed = 0x0002,
            /// <summary>
            /// Indicates that the object is drawn with a check mark.
            /// </summary>
            Checked = 0x0004,
            /// <summary>
            /// Indicates that the object is drawn faintly.
            /// </summary>
            Disabled = 0x0008,
            /// <summary>
            /// Indicates that an outline appears around a box object. This state is used for dialog boxes.
            /// </summary>
            Outlined = 0x0010,
            /// <summary>
            /// Indicates that the object (usually a box) is drawn with a drop shadow
            /// </summary>
            Shadowed = 0x0020
        }

        public enum ObjectColors : byte
        {
            White = 0,
            Black = 1,
            Red = 2,
            Green = 3,
            Blue = 4,
            Cyan = 5,
            Yellow = 6,
            Magenta = 7,
            White2 = 8,
            Black2 = 9,
            LightRed = 10,
            LightGreen = 11,
            LightBlue = 12,
            LightCyan = 13,
            LightYellow = 14,
            LightMagenta = 15
        }

        public enum ObjectFillPattern : byte
        {
            Hollow = 0,
            Dither1 = 1,
            Dither2 = 2,
            Dither3 = 3,
            Dither4 = 4,
            Dither5 = 5,
            Dither6 = 6,
            Solid = 7,
        }
        
        /// <summary>
        /// Mask for border <see cref="ObjectColors"/>
        /// </summary>
        const ushort BorderColorMask = 0xF000;
        /// <summary>
        /// Mask for text <see cref="ObjectColors"/>
        /// </summary>
        const ushort TextColorMask = 0x0F00;
        /// <summary>
        /// If set text is in transparent mode. Replace mode otherwise.
        /// </summary>
        const ushort TransparentColor = 0x0080;
        /// <summary>
        /// Mask for <see cref="ObjectFillPattern"/>
        /// </summary>
        const ushort FillPatternMask = 0x0070;
        /// <summary>
        /// Mask for inside <see cref="ObjectColors"/>
        /// </summary>
        const ushort InsideColorMask = 0x000F;
    }
}