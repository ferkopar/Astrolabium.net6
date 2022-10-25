using System;

namespace SwephCore.Utils
{
    /// <summary>
    /// Trace provider
    /// </summary>
    public interface ITracer
    {

        /// <summary>
        /// Trace a message
        /// </summary>
        void Trace(String message);

    }
}
