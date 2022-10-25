using System;

namespace SwephCore.Date
{

    /// <summary>
    /// Sideral time value
    /// </summary>
    public struct SideralTime
    {
        /// <summary>
        /// Create a new sideral time
        /// </summary>
        public SideralTime(Double time)
            : this() {
            this.Value = time;
        }

        /// <summary>
        /// Convert to string
        /// </summary>
        public override string ToString() {
            return SweFormat.FormatAsTime(Value);
        }

        /// <summary>
        /// Value of the sideral time
        /// </summary>
        public Double Value { get; private set; }
    }

}
