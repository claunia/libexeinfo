//
// Enums.cs
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

namespace libexeinfo
{
    public static partial class GEM
    {
        public enum ObjectColors : byte
        {
            White        = 0,
            Black        = 1,
            Red          = 2,
            Green        = 3,
            Blue         = 4,
            Cyan         = 5,
            Yellow       = 6,
            Magenta      = 7,
            LightGray    = 8,
            Gray         = 9,
            LightRed     = 10,
            LightGreen   = 11,
            LightBlue    = 12,
            LightCyan    = 13,
            LightYellow  = 14,
            LightMagenta = 15
        }

        public enum ObjectColorsRgb : uint
        {
            White        = 0x00FFFFFF,
            Black        = 0x00000000,
            Red          = 0x00FF0000,
            Green        = 0x0000FF00,
            Blue         = 0x000000FF,
            Cyan         = 0x0000FFFF,
            Yellow       = 0x00FFFF00,
            Magenta      = 0x00FF00FF,
            LightGray    = 0x00555555,
            Gray         = 0x00333333,
            LightRed     = 0x00FF3333,
            LightGreen   = 0x0033FF33,
            LightBlue    = 0x003333FF,
            LightCyan    = 0x0033FFFF,
            LightYellow  = 0x00FFFF33,
            LightMagenta = 0x00FF33FF
        }

        public enum ObjectFillPattern : byte
        {
            Hollow  = 0,
            Dither1 = 1,
            Dither2 = 2,
            Dither3 = 3,
            Dither4 = 4,
            Dither5 = 5,
            Dither6 = 6,
            Solid   = 7
        }

        [Flags]
        public enum ObjectFlags : ushort
        {
            /// <summary>
            ///     Indicates that the user can select the object
            /// </summary>
            Selectable = 0x0001,
            /// <summary>
            ///     Indicates that the Form Library will examine the object if the user enters a carriage return. No more than one
            ///     object in a form can be flagged so.
            /// </summary>
            Default = 0x0002,
            /// <summary>
            ///     Indicates that the Form Library will return control to the caller after the exit condition is satisfied. The
            ///     condition is satisfied when the user clicks on the object.
            /// </summary>
            Exit = 0x0004,
            /// <summary>
            ///     Indicates that an object is editable by the user in some way.
            /// </summary>
            Editable = 0x0008,
            /// <summary>
            ///     An object called a radio button.
            /// </summary>
            Rbutton = 0x0010,
            /// <summary>
            ///     Indicates that an object is the last in the tree
            /// </summary>
            Lastob = 0x0020,
            /// <summary>
            ///     Inidicates that the Form Library will return control to the caller afeter the exit condition is satisfied. The
            ///     condition is satisfied when the user clicks while the pointer is over the object.
            /// </summary>
            Touchexit = 0x0040,
            /// <summary>
            ///     Makes a subtree invisible.
            /// </summary>
            Hidetree = 0x0080,
            /// <summary>
            ///     Indicates that the value in <see cref="ObjectNode.ob_spec" /> is a pointer to the actual value.
            /// </summary>
            Indirect = 0x0100,
            /// <summary>
            ///     Under MultiTOS this object creates a three-dimensional object.
            /// </summary>
            Fl3Dind = 0x0200,
            /// <summary>
            ///     Pressing the [Esc] key corresponds to the selection of the object with this flag.
            /// </summary>
            EscCancel = 0x0200,
            /// <summary>
            ///     In 3D operation this object will be treated as an AES background object.
            /// </summary>
            Fl3DBak = 0x0400,
            /// <summary>
            ///     A button with this flag uses a bitmap instead of text.
            /// </summary>
            BitButton = 0x0400,
            /// <summary>
            ///     In 3D operation this object will be treated as an activator.
            /// </summary>
            Fl3DAct = 0x0600,
            /// <summary>
            ///     In MultiTOS and from MagiC 5.10 this flag indicates submenus.
            /// </summary>
            SubMenu = 0x0800,
            /// <summary>
            ///     Pressing the [PAGEUP] key corresponds to the selection of the first object with this flag in the dialog. Pressing
            ///     the [PAGEDOWN] key corresponds to the selection of the last object with this flag.
            /// </summary>
            Scroller = 0x0800,
            /// <summary>
            ///     An object with this flag will be drawn with a 3D border.
            /// </summary>
            Flag3D = 0x1000,
            /// <summary>
            ///     The color of the object is not a color index of the VDI, but an entry in a table with colors for designated
            ///     categories.
            /// </summary>
            UseColorCat = 0x2000,
            /// <summary>
            ///     Sunken 3D background
            /// </summary>
            Fl3DBak2 = 0x4000,
            /// <summary>
            ///     Not implemented
            /// </summary>
            SubMenu2 = 0x8000
        }

        public enum ObjectFont : short
        {
            GdosProportional = 0,
            GdosMonospaced = 1,
            GdosBitmap = 2,
            System = 3,
            Small  = 5
        }

        public enum ObjectJustification : short
        {
            Left   = 0,
            Right  = 1,
            Center = 2
        }

        [Flags]
        public enum ObjectStates : ushort
        {
            /// <summary>
            ///     Indicates that the object is highlighted by being drawn with reversed colors
            /// </summary>
            Selected = 0x0001,
            /// <summary>
            ///     Indicates that an 'X' is drawn in the object.
            /// </summary>
            Crossed = 0x0002,
            /// <summary>
            ///     Indicates that the object is drawn with a check mark.
            /// </summary>
            Checked = 0x0004,
            /// <summary>
            ///     Indicates that the object is drawn faintly.
            /// </summary>
            Disabled = 0x0008,
            /// <summary>
            ///     Indicates that an outline appears around a box object. This state is used for dialog boxes.
            /// </summary>
            Outlined = 0x0010,
            /// <summary>
            ///     Indicates that the object (usually a box) is drawn with a drop shadow
            /// </summary>
            Shadowed = 0x0020,
            /// <summary>
            ///     In PC-GEM ignores icon background. As of MagiC 3 controls the underscoring of character strings.
            /// </summary>
            Whitebak = 0x0040,
            /// <summary>
            ///     Indicates that the object is to be drawn with a 3D effect.
            /// </summary>
            Draw3D = 0x0080,
            /// <summary>
            ///     An object with this status will be surrounded by a dashed line.
            /// </summary>
            Highlighted = 0x0100,
            /// <summary>
            ///     Setting this state undoes the line by <see cref="Highlighted" />
            /// </summary>
            Unhighlighted = 0x0200,
            Underline     = 0x0F00,
            Xstate        = 0xF000
        }

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
            ///     A programmed defined object. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a
            ///     <see cref="ApplicationBlock" />
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
            ///     An object that describes a color icon. Its <see cref="ObjectNode.ob_spec" /> is a pointer to an
            ///     <see cref="ColorIconBlock" /> structure.
            /// </summary>
            G_CICON = 33,
            /// <summary>
            ///     An object that describes a cycle button (a button which alters its text cyclically when clicked on).
            ///     Its <see cref="ObjectNode.ob_spec" /> is a pointer to a <see cref="SwInfoBlock" /> structure.
            /// </summary>
            G_SWBUTTON = 34,
            /// <summary>
            ///     An object that describes a popup menu. Its <see cref="ObjectNode.ob_spec" /> is a pointer to a
            ///     <see cref="PopInfoBlock" /> structure.
            /// </summary>
            G_POPUP = 35,
            /// <summary>
            ///     This object is user internally by MagiC to depict window titles.
            /// </summary>
            G_WINTITLE = 36,
            /// <summary>
            ///     As of MagiC 5.20 an editable object implemented in a shared library. Its <see cref="ObjectNode.ob_spec" /> is a
            ///     pointer to the object.
            /// </summary>
            G_EDIT = 37,
            /// <summary>
            ///     Similar to <see cref="G_STRING" />, but any keyboard shortcut present is split off and output ranged right.
            /// </summary>
            G_SHORTCUT = 38,
            /// <summary>
            ///     Scrolling list
            /// </summary>
            G_SLIST = 39
        }

        /// <summary>
        ///     Mask for border <see cref="ObjectColors" />
        /// </summary>
        public const ushort BorderColorMask = 0xF000;
        /// <summary>
        ///     Mask for text <see cref="ObjectColors" />
        /// </summary>
        public const ushort TextColorMask = 0x0F00;
        /// <summary>
        ///     If set text is in transparent mode. Replace mode otherwise.
        /// </summary>
        public const ushort TransparentColor = 0x0080;
        /// <summary>
        ///     Mask for <see cref="ObjectFillPattern" />
        /// </summary>
        public const ushort FillPatternMask = 0x0070;
        /// <summary>
        ///     Mask for inside <see cref="ObjectColors" />
        /// </summary>
        public const ushort InsideColorMask = 0x000F;
    }
}