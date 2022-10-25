using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwephCore.Date
{

    /// <summary>
    /// Interface for DeltaT records provider
    /// </summary>
    public interface IDeltaTRecordProvider
    {
        /// <summary>
        /// Read all records
        /// </summary>
        IEnumerable<DeltaTRecord> GetRecords();
    }

}
