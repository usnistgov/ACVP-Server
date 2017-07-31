using System;
using System.Collections.Generic;

namespace NIST.CVP.Math.Domain
{
    /// <summary>
    /// Domain segment that represents a single value
    /// </summary>
    public class ValueDomainSegment : IDomainSegment
    {
        public const int _MAXIMUM_ALLOWED_NUMBER = (1 << 16) * 8;

        private int _value;

        public RangeMinMax RangeMinMax => new RangeMinMax()
        {
            Minimum = _value,
            Maximum = _value
        };

        public RangeDomainSegmentOptions SegmentValueOptions
        {
            set
            {
                // Doesn't apply to single value Segment
            }
        }

        /// <summary>
        /// Constructor - takes in the value for the domain segment
        /// </summary>
        /// <param name="value">The single value belonging to the domain segment</param>
        public ValueDomainSegment(long value)
        {
            _value = value > _MAXIMUM_ALLOWED_NUMBER ? _MAXIMUM_ALLOWED_NUMBER : (int)value;
        }
        
        public void SetMaximumAllowedValue(int value)
        {
            if (value < _value)
            {
                _value = value;
            }
        }

        /// <summary>
        /// Determines if the value is within the domain segment
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsWithinDomain(int value)
        {
            return value == _value;
        }

        /// <summary>
        /// Returns the values of the domain segment
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IEnumerable<int> GetValues(int quantity)
        {
            return new List<int>() { _value };
        }

        /// <summary>
        /// Get values from the <see cref="IDomainSegment" />, with a minimum of <see cref="min" />
        /// and a maximum <see cref="max" />, up to the quantity
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="quantity">The maximum number of values to return from the <see cref="T:NIST.CVP.Math.Domain.IDomainSegment" /></param>
        /// <returns></returns>
        public IEnumerable<int> GetValues(int min, int max, int quantity)
        {
            List<int> values = new List<int>();

            if (min == _value || max == _value)
            {
                values.Add(_value);
            }

            return values;
        }
    }
}
