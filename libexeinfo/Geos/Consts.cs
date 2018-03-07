//
// Consts.cs
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
// THE SOFTWARE.namespace libexeinfo.Geos

namespace libexeinfo
{
    public partial class Geos
    {
        /// <summary>GEOS file identification "magic"</summary>
        const uint GEOS_ID = 0x53CF45C7;
        /// <summary>GEOS2 file identification "magic"</summary>
        const uint GEOS2_ID = 0x53C145C7;

        const int GEOS_TOKENLEN = 4;
        /// <summary>Length of filename</summary>
        const int GEOS_LONGNAME = 36;
        /// <summary>Length of user file info</summary>
        const int GEOS_INFO = 100;
        /// <summary>Length of internal filename</summary>
        const int GEOS_FNAME = 8;
        /// <summary>Length of internal extension</summary>
        const int GEOS_FEXT = 4;
    }
}