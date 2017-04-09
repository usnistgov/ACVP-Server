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
    }
}
