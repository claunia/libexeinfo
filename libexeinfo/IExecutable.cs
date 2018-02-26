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
    }
}