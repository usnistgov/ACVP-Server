using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;

namespace NIST.CVP.ACVTS.Libraries.Math.Domain
{
    /// <summary>
    /// Domain segment that represents a single value
    /// </summary>
    public class ValueDomainSegment : IDomainSegment
    {
        private int _value;

        public RangeMinMax RangeMinMax => new RangeMinMax()
        {
            Minimum = _value,
            Maximum = _value
        };

        /// <summary>
        /// Constructor - takes in the value for the domain segment
        /// </summary>
        /// <param name="value">The single value belonging to the domain segment</param>
        public ValueDomainSegment(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Returns a deep copy of the segment
        /// </summary>
        /// <returns></returns>
        public IDomainSegment GetDeepCopy()
        {
            return new ValueDomainSegment(_value);
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
        public IEnumerable<int> GetSequentialValues(int quantity)
        {
            return new List<int>() { _value };
        }

        // TODO - GET VALUES - WHAT IS THIS?
        /// <summary>
        /// Returns the values of the domain segment
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IEnumerable<int> GetRandomValues(int quantity)
        {
            return new List<int>() { _value };
        }
        
        public IEnumerable<int> GetSequentialValues(Func<int, bool> condition, int quantity)
        {
            return new List<int> { _value }.Where(condition);
        }

        public IEnumerable<int> GetRandomValues(Func<int, bool> condition, int quantity)
        {
            return new List<int> { _value }.Where(condition);
        }
        
        /// <summary>
        /// Get values from the <see cref="IDomainSegment" />, with a minimum of <see cref="min" />
        /// and a maximum <see cref="max" />, up to the quantity
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="quantity">The maximum number of values to return from the <see cref="T:NIST.CVP.ACVTS.Libraries.Math.Domain.IDomainSegment" /></param>
        /// <param name="randomizeReturn">Force the return values to be randomized <see cref="T:NIST.CVP.ACVTS.Libraries.Math.Domain.IDomainSegment" /></param>
        /// <returns></returns>
        public IEnumerable<int> GetSequentialValues(int min, int max, int quantity)
        {
            List<int> values = new List<int>();

            if (_value >= min && _value <= max)
            {
                values.Add(_value);
            }

            return values;
        }
        
        /// <summary>
        /// Get values from the <see cref="IDomainSegment" />, with a minimum of <see cref="min" />
        /// and a maximum <see cref="max" />, up to the quantity
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="quantity">The maximum number of values to return from the <see cref="T:NIST.CVP.ACVTS.Libraries.Math.Domain.IDomainSegment" /></param>
        /// <param name="randomizeReturn">Force the return values to be randomized <see cref="T:NIST.CVP.ACVTS.Libraries.Math.Domain.IDomainSegment" /></param>
        /// <returns></returns>
        public IEnumerable<int> GetRandomValues(int min, int max, int quantity)
        {
            List<int> values = new List<int>();

            if (_value >= min && _value <= max)
            {
                values.Add(_value);
            }

            return values.Shuffle();
        }
    }
}
