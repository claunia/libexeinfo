//
// IExecutable.cs
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

using System.Collections.Generic;
using System.IO;

namespace libexeinfo
{
    public interface IExecutable
    {
        /// <summary>
        ///     If <c>true</c> the executable is recognized by this instance
        /// </summary>
        bool Recognized { get; }
        /// <summary>
        ///     Name of executable format
        /// </summary>
        string Type { get; }
        /// <summary>
        ///     The <see cref="Stream" /> that contains the executable represented by this instance
        /// </summary>
        Stream BaseStream { get; }
        /// <summary>
        ///     If <c>true</c> the executable is for a big-endian architecture
        /// </summary>
        bool IsBigEndian { get; }
        /// <summary>
        ///     General description of executable contents
        /// </summary>
        string Information { get; }
        /// <summary>
        ///     Architectures that the executable can run on
        /// </summary>
        IEnumerable<Architecture> Architectures { get; }
        /// <summary>
        ///     Operating system the executable requires to run on
        /// </summary>
        OperatingSystem RequiredOperatingSystem { get; }
        /// <summary>
        ///     List of all strings available in the executable resources, if any
        /// </summary>
        IEnumerable<string> Strings { get; }
        /// <summary>
        ///     List of all file segments, if any
        /// </summary>
        IEnumerable<Segment> Segments { get; }
    }
}