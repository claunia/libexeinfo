//
// FreeBSD.cs
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

namespace libexeinfo
{
    public partial class ELF
    {
        static FreeBSDTag DecodeFreeBSDTag(uint desc)
        {
            FreeBSDTag tag = new FreeBSDTag {major = desc / 100000};

            if(desc == 460002)
            {
                tag.major    = 4;
                tag.minor    = 6;
                tag.revision = 2;
            }
            else if(desc < 460100)
            {
                tag.minor = desc / 10000 % 10;
                if(desc          / 1000  % 10 > 0) tag.revision = desc / 1000 % 10;
            }
            else if(desc < 500000)
            {
                tag.minor = desc / 10000 % 10 + desc                        / 1000 % 10;
                if(desc                  / 10 % 10 > 0) tag.revision = desc / 10   % 10;
            }
            else
            {
                tag.minor = desc / 1000 % 100;
                if(desc          / 10   % 10 > 0) tag.revision = desc / 10 % 10;
            }

            return tag;
        }
    }
}