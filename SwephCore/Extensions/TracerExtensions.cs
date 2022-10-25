using System;
using SwephCore.Utils;

namespace SwephCore.Extensions
{
    /// <summary>
    /// Extensions for ITracer
    /// </summary>
    public static class TracerExtensions
    {
        /// <summary>
        /// Trace a formatted message
        /// </summary>
        public static void Trace(this ITracer tracer, String message, params object[] args)
        {
            if (tracer != null)
            {
                if (args != null && args.Length > 0)
                    message = String.Format(message, args);
                tracer.Trace(message);
            }
        }

    }
}
