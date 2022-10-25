using System;
using System.IO;

namespace SwephCore.Persit
{

    /// <summary>
    /// Arguments for load file event
    /// </summary>
    public class LoadFileEventArgs : EventArgs
    {
        /// <summary>
        /// Create new arguments
        /// </summary>
        public LoadFileEventArgs(String fileName)
        {
            this.FileName = fileName;
            this.File = null;
        }

        /// <summary>
        /// File to load
        /// </summary>
        public String FileName { get; private set; }

        /// <summary>
        /// Stream
        /// </summary>
        public Stream File { get; set; }

    }

}
