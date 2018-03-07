//
// Info.cs
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
// THE SOFTWARE.namespace libexeinfo.Geos

using System.Text;

namespace libexeinfo
{
    public partial class Geos
    {
        public string Information
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("GEOS executable{0}", isNewHeader ? " v2" : "").AppendLine();

                sb.AppendFormat("\tClass: {0}", isNewHeader ? $"{header2.type}" : $"{header.type}").AppendLine();
                sb.AppendFormat("\tType: {0}", isNewHeader ? applicationHeader2.type : applicationHeader.type)
                  .AppendLine();
                sb.AppendFormat("\tAttributes: {0}",
                                isNewHeader ? applicationHeader2.attributes : applicationHeader.attributes)
                  .AppendLine();
                sb.AppendFormat("\tName: {0}", StringHandlers.CToString(isNewHeader ? header2.name : header.name))
                  .AppendLine();
                sb.AppendFormat("\tInternal name: \"{0}.{1}\"",
                                isNewHeader
                                    ? StringHandlers.CToString(applicationHeader2.name).Trim()
                                    : StringHandlers.CToString(applicationHeader.name).Trim(),
                                isNewHeader
                                    ? StringHandlers.CToString(applicationHeader2.extension).Trim()
                                    : StringHandlers.CToString(applicationHeader.extension).Trim()).AppendLine();
                sb.AppendFormat("\tVersion: {0}",
                                isNewHeader
                                    ? $"{header2.release.major}.{header2.release.minor}.{header2.release.change}.{header2.release.engineering}"
                                    : $"{header.release.major}.{header.release.minor}.{header.release.change}.{header.release.engineering}")
                  .AppendLine();
                sb.AppendFormat("\tCopyright string: {0}",
                                StringHandlers.CToString(isNewHeader ? header2.copyright : header.copyright))
                  .AppendLine();
                sb.AppendFormat("\tInformational string: {0}",
                                StringHandlers.CToString(isNewHeader ? header2.info : header.info)).AppendLine();
                sb.AppendFormat("\tProtocol: {0}",
                                isNewHeader
                                    ? $"{header2.protocol.major}.{header2.protocol.minor}"
                                    : $"{header.protocol.major}.{header.protocol.minor}").AppendLine();
                sb.AppendFormat("\tApplication token: \"{0}\" id {1}",
                                isNewHeader
                                    ? StringHandlers.CToString(header2.application.str)
                                    : StringHandlers.CToString(header.creator.str),
                                isNewHeader ? header2.application.manufacturer : header.creator.manufacturer)
                  .AppendLine();
                sb.AppendFormat("\tToken: \"{0}\" id {1}",
                                isNewHeader
                                    ? StringHandlers.CToString(header2.token.str)
                                    : StringHandlers.CToString(header.token.str),
                                isNewHeader ? header2.token.manufacturer : header.token.manufacturer).AppendLine();

                sb.AppendFormat("\tSegments: {0}",
                                isNewHeader ? applicationHeader2.segments : applicationHeader.segments).AppendLine();
                sb.AppendFormat("\tImported libraries: {0}",
                                isNewHeader ? applicationHeader2.imports : applicationHeader.imports).AppendLine();
                sb.AppendFormat("\tExported entry points: {0}",
                                isNewHeader ? applicationHeader2.exports : applicationHeader.exports).AppendLine();

                sb.AppendFormat("\t{0} imports:", imports.Length).AppendLine();
                for(int i = 0; i < imports.Length; i++)
                    sb.AppendFormat("\t\tImport \"{0}\", attributes {1}, protocol {2}.{3}",
                                    StringHandlers.CToString(imports[i].name).Trim(), imports[i].attributes,
                                    imports[i].protocol.major, imports[i].protocol.minor).AppendLine();

                sb.AppendFormat("\t{0} exports:", exports.Length).AppendLine();
                for(int i = 0; i < exports.Length; i++)
                    sb.AppendFormat("\t\tExported entry point in segment {0} offset {1}", exports[i].segment,
                                    exports[i].offset).AppendLine();

                sb.AppendFormat("\t{0} segments:", segments.Length).AppendLine();
                for(int i = 0; i < segments.Length; i++)
                    sb
                       .AppendFormat("\t\tSegment {0} starts at {1} runs for {2} bytes with flags {3} has {4} relocations",
                                     i, segments[i].offset, segments[i].length, segments[i].flags,
                                     segments[i].relocs_length / 4).AppendLine();

                return sb.ToString();
            }
        }
    }
}