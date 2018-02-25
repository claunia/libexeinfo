//
// Consts.cs
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
    public partial class COFF
    {
        public const ushort STMAGIC = 0x101;
        public const ushort OMAGIC  = 0x104;
	    /// <summary>
	    ///     dirty text and data image, can't share
	    /// </summary>
	    public const ushort JMAGIC = 0x107;
	    /// <summary>
	    ///     dirty text segment, data aligned
	    /// </summary>
	    public const ushort DMAGIC = 0x108;
	    /// <summary>
	    ///     The proper magic number for executables
	    /// </summary>
	    public const ushort ZMAGIC = 0x10b;
	    /// <summary>
	    ///     shared library header
	    /// </summary>
	    public const ushort SHMAGIC = 0x123;

	    /// <summary>
	    ///     Alpha architecture information
	    /// </summary>
	    public readonly string SectionAlpha = ".arch";
	    /// <summary>
	    ///     Uninitialized data
	    /// </summary>
	    public readonly string SectionBss = ".bss";
	    /// <summary>
	    ///     Initialized data
	    /// </summary>
	    public readonly string SectionData = ".data";
	    /// <summary>
	    ///     Debug information
	    /// </summary>
	    public readonly string SectionDebug = ".debug";
	    /// <summary>
	    ///     A .drectve section consists of a string of ASCII text. This string is
	    ///     a series of linker options (each option containing hyphen, option
	    ///     name, and any appropriate attribute) separated by spaces.The
	    ///     .drectve section must not have relocations or line numbers.
	    /// </summary>
	    public readonly string SectionDirectives = ".drectve";
	    /// <summary>
	    ///     Exception information
	    /// </summary>
	    public readonly string SectioneXception = ".xdata";
	    /// <summary>
	    ///     Exception information
	    /// </summary>
	    public readonly string SectionException = ".pdata";
	    /// <summary>
	    ///     Executable code
	    /// </summary>
	    public readonly string SectionExecutable = ".text";
	    /// <summary>
	    ///     The export data section, named .edata, contains information about
	    ///     symbols that other images can access through dynamic linking.
	    ///     Exports are generally found in DLLs, but DLLs can import symbols as
	    ///     well.
	    /// </summary>
	    public readonly string SectionExport = ".edata";
	    /// <summary>
	    ///     Import tables
	    /// </summary>
	    public readonly string SectionImport = ".idata";
	    /// <summary>
	    ///     Read-only initialized data
	    /// </summary>
	    public readonly string SectionReadOnly = ".rdata";
	    /// <summary>
	    ///     Image relocations
	    /// </summary>
	    public readonly string SectionRelocations = ".reloc";
	    /// <summary>
	    ///     Resource directory
	    /// </summary>
	    public readonly string SectionResource = ".rsrc";
	    /// <summary>
	    ///     Thread-local storage
	    /// </summary>
	    public readonly string SectionTls = ".tls";
    }
}