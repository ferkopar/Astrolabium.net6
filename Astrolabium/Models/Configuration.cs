
using System.Collections.Generic;

namespace Astrolabium.Models
{

    /// <summary>
    /// Sweph configuration
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// New configuration
        /// </summary>
        public Configuration() {
            SearchPaths = new List<string>();
            SearchPaths.Add(".\\Ephe\\");
        }
        /// <summary>
        /// Liste of file search folders
        /// </summary>
        public List<string> SearchPaths { get; }
    }

}
