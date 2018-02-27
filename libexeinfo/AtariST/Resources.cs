//
// AtariST.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace libexeinfo
{
    public partial class AtariST : IExecutable
    {
        static TreeObjectNode ProcessResourceObject(IList<ObjectNode> nodes, ref List<short> knownNodes, short nodeNumber)
        {
            TreeObjectNode node = new TreeObjectNode
            {
                type   = (ObjectTypes)nodes[nodeNumber].ob_type,
                flags  = (ObjectFlags)nodes[nodeNumber].ob_flags,
                state  = (ObjectStates)nodes[nodeNumber].ob_state,
                data   = nodes[nodeNumber].ob_spec,
                x      = nodes[nodeNumber].ob_x,
                y      = nodes[nodeNumber].ob_y,
                width  = nodes[nodeNumber].ob_width,
                height = nodes[nodeNumber].ob_height
            };

            knownNodes.Add(nodeNumber);

            if(nodes[nodeNumber].ob_head > 0 && !knownNodes.Contains(nodes[nodeNumber].ob_head))
                node.child = ProcessResourceObject(nodes, ref knownNodes, nodes[nodeNumber].ob_head);

            if(nodes[nodeNumber].ob_next > 0 && !knownNodes.Contains(nodes[nodeNumber].ob_next))
                node.sibling = ProcessResourceObject(nodes, ref knownNodes, nodes[nodeNumber].ob_next);

            return node;
        }
    }
}