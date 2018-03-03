//
// GetStrings.cs
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
using System.Text;

namespace libexeinfo
{
    public partial class NE
    {
        public static IEnumerable<string> GetWindowsStrings(ResourceType resource)
        {
            List<string> strings = new List<string>();

            foreach(Resource r in resource.resources) strings.AddRange(GetWindowsStrings(r.data));

            return strings;
        }

        public static IEnumerable<string> GetWindowsStrings(byte[] data)
        {
            List<string> strings = new List<string>();

            int pos = 0;
            while(pos < data.Length)
            {
                byte len = data[pos++];

                if(len == 0) break;

                byte[] buffer = new byte[len];
                Array.Copy(data, pos, buffer, 0, len);
                string str = Encoding.Default.GetString(buffer);
                if(!string.IsNullOrWhiteSpace(str)) strings.Add(str);
                pos += len;
            }

            return strings;
        }

        public static IEnumerable<string> GetOs2Strings(ResourceType resource)
        {
            List<string> strings = new List<string>();

            foreach(Resource r in resource.resources) strings.AddRange(GetOs2Strings(r.data));

            return strings;
        }

        public static IEnumerable<string> GetOs2Strings(byte[] data)
        {
            List<string> strings = new List<string>();

            // Skip resource header
            int pos = 2;
            while(pos < data.Length)
            {
                byte len = data[pos++];

                if(len == 0) break;

                byte[] buffer = new byte[len];
                Array.Copy(data, pos, buffer, 0, len);
                string str = StringHandlers.CToString(buffer, Encoding.Default);
                if(!string.IsNullOrWhiteSpace(str)) strings.Add(str);
                pos += len;
            }

            return strings;
        }
    }
}