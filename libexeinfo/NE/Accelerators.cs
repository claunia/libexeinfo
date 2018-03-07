//
// Accelerator.cs
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
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace libexeinfo
{
    public partial class NE
    {
        [Flags]
        public enum Os2AcceleratorFlags : ushort
        {
            AF_CHAR       = 0x0001,
            AF_VIRTUALKEY = 0x0002,
            AF_SCANCODE   = 0x0004,
            AF_SHIFT      = 0x0008,
            AF_CONTROL    = 0x0010,
            AF_ALT        = 0x0020,
            AF_LONEKEY    = 0x0040,
            AF_SYSCOMMAND = 0x0100,
            AF_HELP       = 0x0200
        }

        public enum Os2VirtualKey : ushort
        {
            VK_BUTTON1   = 0x01,
            VK_BUTTON2   = 0x02,
            VK_BUTTON3   = 0x03,
            VK_BREAK     = 0x04,
            VK_BACKSPACE = 0x05,
            VK_TAB       = 0x06,
            VK_BACKTAB   = 0x07,
            VK_NEWLINE   = 0x08,
            VK_SHIFT     = 0x09,
            VK_CTRL      = 0x0A,
            VK_ALT       = 0x0B,
            VK_ALTGRAF   = 0x0C,
            VK_PAUSE     = 0x0D,
            VK_CAPSLOCK  = 0x0E,
            VK_ESC       = 0x0F,
            VK_SPACE     = 0x10,
            VK_PAGEUP    = 0x11,
            VK_PAGEDOWN  = 0x12,
            VK_END       = 0x13,
            VK_HOME      = 0x14,
            VK_LEFT      = 0x15,
            VK_UP        = 0x16,
            VK_RIGHT     = 0x17,
            VK_DOWN      = 0x18,
            VK_PRINTSCRN = 0x19,
            VK_INSERT    = 0x1A,
            VK_DELETE    = 0x1B,
            VK_SCRLLOCK  = 0x1C,
            VK_NUMLOCK   = 0x1D,
            VK_ENTER     = 0x1E,
            VK_SYSRQ     = 0x1F,
            VK_F1        = 0x20,
            VK_F2        = 0x21,
            VK_F3        = 0x22,
            VK_F4        = 0x23,
            VK_F5        = 0x24,
            VK_F6        = 0x25,
            VK_F7        = 0x26,
            VK_F8        = 0x27,
            VK_F9        = 0x28,
            VK_F10       = 0x29,
            VK_F11       = 0x2A,
            VK_F12       = 0x2B,
            VK_F13       = 0x2C,
            VK_F14       = 0x2D,
            VK_F15       = 0x2E,
            VK_F16       = 0x2F,
            VK_F17       = 0x30,
            VK_F18       = 0x31,
            VK_F19       = 0x32,
            VK_F20       = 0x33,
            VK_F21       = 0x34,
            VK_F22       = 0x35,
            VK_F23       = 0x36,
            VK_F24       = 0x37
        }

        [Flags]
        public enum WinAcceleratorFlags : byte
        {
            VirtualKey = 0x01,
            NoInvert   = 0x02,
            Shift      = 0x04,
            Control    = 0x08,
            Alt        = 0x10,
            Last       = 0x80
        }

        public enum WinVirtualKey : byte
        {
            VK_LBUTTON   = 0x01,
            VK_RBUTTON   = 0x02,
            VK_CANCEL    = 0x03,
            VK_MBUTTON   = 0x04,
            VK_BACK      = 0x08,
            VK_TAB       = 0x09,
            VK_CLEAR     = 0x0C,
            VK_RETURN    = 0x0D,
            VK_SHIFT     = 0x10,
            VK_CONTROL   = 0x11,
            VK_MENU      = 0x12,
            VK_PAUSE     = 0x13,
            VK_CAPITAL   = 0x14,
            VK_ESCAPE    = 0x1B,
            VK_SPACE     = 0x20,
            VK_PRIOR     = 0x21,
            VK_NEXT      = 0x22,
            VK_END       = 0x23,
            VK_HOME      = 0x24,
            VK_LEFT      = 0x25,
            VK_UP        = 0x26,
            VK_RIGHT     = 0x27,
            VK_DOWN      = 0x28,
            VK_SELECT    = 0x29,
            VK_PRINT     = 0x2A,
            VK_EXECUTE   = 0x2B,
            VK_SNAPSHOT  = 0x2C,
            VK_INSERT    = 0x2D,
            VK_DELETE    = 0x2E,
            VK_HELP      = 0x2F,
            VK_0         = 0x30,
            VK_1         = 0x31,
            VK_2         = 0x32,
            VK_3         = 0x33,
            VK_4         = 0x34,
            VK_5         = 0x35,
            VK_6         = 0x36,
            VK_7         = 0x37,
            VK_8         = 0x38,
            VK_9         = 0x39,
            VK_A         = 0x41,
            VK_B         = 0x42,
            VK_C         = 0x43,
            VK_D         = 0x44,
            VK_E         = 0x45,
            VK_F         = 0x46,
            VK_G         = 0x47,
            VK_H         = 0x48,
            VK_I         = 0x49,
            VK_J         = 0x4A,
            VK_K         = 0x4B,
            VK_L         = 0x4C,
            VK_M         = 0x4D,
            VK_N         = 0x4E,
            VK_O         = 0x4F,
            VK_P         = 0x50,
            VK_Q         = 0x51,
            VK_R         = 0x52,
            VK_S         = 0x53,
            VK_T         = 0x54,
            VK_U         = 0x55,
            VK_V         = 0x56,
            VK_W         = 0x57,
            VK_X         = 0x58,
            VK_Y         = 0x59,
            VK_Z         = 0x5A,
            VK_NUMPAD0   = 0x60,
            VK_NUMPAD1   = 0x61,
            VK_NUMPAD2   = 0x62,
            VK_NUMPAD3   = 0x63,
            VK_NUMPAD4   = 0x64,
            VK_NUMPAD5   = 0x65,
            VK_NUMPAD6   = 0x66,
            VK_NUMPAD7   = 0x67,
            VK_NUMPAD8   = 0x68,
            VK_NUMPAD9   = 0x69,
            VK_MULTIPLY  = 0x6A,
            VK_ADD       = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT  = 0x6D,
            VK_DECIMAL   = 0x6E,
            VK_DIVIDE    = 0x6F,
            VK_F1        = 0x70,
            VK_F2        = 0x71,
            VK_F3        = 0x72,
            VK_F4        = 0x73,
            VK_F5        = 0x74,
            VK_F6        = 0x75,
            VK_F7        = 0x76,
            VK_F8        = 0x77,
            VK_F9        = 0x78,
            VK_F10       = 0x79,
            VK_F11       = 0x7A,
            VK_F12       = 0x7B,
            VK_F13       = 0x7C,
            VK_F14       = 0x7D,
            VK_F15       = 0x7E,
            VK_F16       = 0x7F,
            VK_F17       = 0x80,
            VK_F18       = 0x81,
            VK_F19       = 0x82,
            VK_F20       = 0x83,
            VK_F21       = 0x84,
            VK_F22       = 0x85,
            VK_F23       = 0x86,
            VK_F24       = 0x87,
            VK_NUMLOCK   = 0x90,
            VK_SCROLL    = 0x91
        }

        public static WinAccelerator[] GetWinAccelerators(byte[] data)
        {
            int                  pos          = 0;
            List<WinAccelerator> accelerators = new List<WinAccelerator>();

            while(pos + 8 < data.Length)
            {
                byte[] accelBytes = new byte[Marshal.SizeOf(typeof(WinAccelerator))];
                Array.Copy(data, pos, accelBytes, 0, accelBytes.Length);
                WinAccelerator accelerator =
                    BigEndianMarshal.ByteArrayToStructureLittleEndian<WinAccelerator>(accelBytes);

                accelerators.Add(accelerator);

                if(accelerator.Flags.HasFlag(WinAcceleratorFlags.Last)) break;

                pos += accelBytes.Length;
            }

            return accelerators.ToArray();
        }

        public static Os2AcceleratorTable GetOs2Accelerators(byte[] data)
        {
            int                 pos   = 4;
            Os2AcceleratorTable table = new Os2AcceleratorTable
            {
                Count    = BitConverter.ToUInt16(data, 0),
                CodePage = BitConverter.ToUInt16(data, 2)
            };
            table.Accelerators = new Os2Accelerator[table.Count];

            for(int i = 0; i < table.Count; i++)
            {
                byte[] accelBytes = new byte[Marshal.SizeOf(typeof(Os2Accelerator))];
                if(pos + accelBytes.Length > data.Length) break;

                Array.Copy(data, pos, accelBytes, 0, accelBytes.Length);

                table.Accelerators[i] =  BigEndianMarshal.ByteArrayToStructureLittleEndian<Os2Accelerator>(accelBytes);
                pos                   += accelBytes.Length;
            }

            return table;
        }

        public struct Os2AcceleratorTable
        {
            /// <summary>
            ///     How many accelerators are included in this table
            /// </summary>
            public ushort Count;
            /// <summary>
            ///     Table's codepage
            /// </summary>
            public ushort CodePage;
            /// <summary>
            ///     Array of accelerators
            /// </summary>
            public Os2Accelerator[] Accelerators;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Os2Accelerator
        {
            /// <summary>
            ///     Accelerator type
            /// </summary>
            public Os2AcceleratorFlags Type;
            /// <summary>
            ///     Accelerator key
            /// </summary>
            public Os2VirtualKey Key;
            /// <summary>
            ///     Accelerator command
            /// </summary>
            public ushort Command;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WinAccelerator
        {
            /// <summary>
            ///     Accelerator flags
            /// </summary>
            public WinAcceleratorFlags Flags;
            /// <summary>
            ///     Accelerator key
            /// </summary>
            public WinVirtualKey Key;
            /// <summary>
            ///     Padding
            /// </summary>
            public byte Padding;
            /// <summary>
            ///     Accelerator command
            /// </summary>
            public byte Command;
        }
    }
}