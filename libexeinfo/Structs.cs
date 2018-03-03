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

namespace libexeinfo
{
    public struct OperatingSystem
    {
        public string Name;
        public int    MajorVersion;
        public int    MinorVersion;
        public string Subsystem;
    }

    public class Segment
    {
        /// <summary>
        /// Standardized segment type name: .text, .data, .rsrc, .bss, etc.
        /// </summary>
        public string Name;
        /// <summary>
        /// String containing the name of the flags that the executable set accordingly.
        /// </summary>
        public string Flags;
        /// <summary>
        /// Offset from start of file where the segment starts. 0 if segment is memory only
        /// </summary>
        public long Offset;
        /// <summary>
        /// Size in bytes of the segment.
        /// </summary>
        public long Size;
    }
}