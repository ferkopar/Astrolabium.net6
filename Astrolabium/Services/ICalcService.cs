using Astrolabium.Models;

namespace Astrolabium.Services
{
    /// <summary>
    /// Interface of a Sweph calculation service
    /// </summary>
    public interface ICalcService
    {

        /// <summary>
        /// Make calculation
        /// </summary>
        EphemerisResult Calculate(Configuration config, InputCalculation input);

    }
}
