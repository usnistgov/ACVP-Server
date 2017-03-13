using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Represents a range of values
    /// </summary>
    public class Range
    {
        public const int _DEFAULT_MAXIMUM_NUMBER_OF_VALUES_TO_RETURN = 100;

        /// <summary>
        /// The minimum value within the <see cref="Range"/>
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// The Maximum value within the <see cref="Range"/>
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// Get the minimum and maximum values so they can be enumerated.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetMinMaxAsEnumerable()
        {
            List<int> values = new List<int>();

            values.Add(Min);
            values.Add(Max);

            return values;
        }

        /// <summary>
        /// Gets the values for the minimum through maximum.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetValues(int maxToReturn = _DEFAULT_MAXIMUM_NUMBER_OF_VALUES_TO_RETURN)
        {
            List<int> values = new List<int>();

            int numberToReturn = Max - Min < maxToReturn ? Max - Min : maxToReturn;

            for (int i = Min; i <= numberToReturn; i++)
            {
                values.Add(i);
            }

            return values;
        }
    }
}
