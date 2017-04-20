using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NIST.CVP.Math.Domain
{
    /// <summary>
    /// Domain segment that represents a range of values (a min and max and values in between based on step)
    /// </summary>
    public class RangeDomainSegment : IDomainSegment
    {
        private int _min;
        private int _max;
        private readonly int _increment;
        private readonly IRandom800_90 _random;
        private readonly List<int> _returnedValues = new List<int>();
        private bool _valuesHaveBeenGenerated;
        private RangeDomainSegmentOptions _segmentValueOptions = RangeDomainSegmentOptions.Sequential;

        public const int _MAXIMUM_ALLOWED_RETURNS = 1000;
        public const int _MAXIMUM_ALLOWED_NUMBER_TO_RANDOMLY_GENERATE = (1 << 16) * 8;
        public const string _NOT_SUPPORTED_OPERATION =
            "Values have already been generated, cannot change generation options once this occurs.";

        /// <summary>
        /// The maximum number of values belonging to the segment
        /// </summary>
        public int MaxNumberOfValuesInSegment
        {
            get
            {
                var availableNumbersToSegment = ((_max - _min) / _increment) + 1;

                if (availableNumbersToSegment > _MAXIMUM_ALLOWED_RETURNS)
                {
                    return _MAXIMUM_ALLOWED_RETURNS;
                }
                else
                {
                    return (int)availableNumbersToSegment;
                }
            }
        }

        /// <summary>
        /// Specifies how to retrieve values from the segment
        /// </summary>
        public RangeDomainSegmentOptions SegmentValueOptions
        {
            get
            {
                return _segmentValueOptions;
            }
            set
            {
                if (_valuesHaveBeenGenerated)
                {
                    throw new NotSupportedException(_NOT_SUPPORTED_OPERATION);
                }

                _segmentValueOptions = value;
            }
        }

        /// <summary>
        /// The Minimum and Maximimum values for the domain segment.
        /// </summary>
        public RangeMinMax RangeMinMax => new RangeMinMax()
        {
            Minimum = _min,
            Maximum = _max,
            Increment = _increment
        };

        /// <summary>
        /// Constructor - takes in the min and max values for the domain segment
        /// </summary>
        /// <param name="random">The random number generator implementation</param>
        /// <param name="min">The minimum value for the segment</param>
        /// <param name="max">The maximum value for the segment</param>
        /// <param name="increment">The "increment" of the range, e.g. min: 2, max: 9, step: 2, return 2, 4, 6, 8</param>
        public RangeDomainSegment(IRandom800_90 random, long min, long max, int increment = 1)
        {
            if (min >= max)
            {
                throw new ArgumentException($"{nameof(min)} must be less than {nameof(max)}");
            }
            if (increment < 1)
            {
                throw new ArgumentException($"{nameof(increment)} must be 1 or greater.");
            }
            if (max - min < increment)
            {
                throw new ArgumentException($"{nameof(increment)} cannot exceed the difference between {nameof(min)} and {nameof(max)}");
            }
            if ((max - min) % increment != 0)
            {
                throw new ArgumentException($"{nameof(min)} - {nameof(max)} mod {nameof(increment)} must be 0.");
            }

            // We don't want to generate extremely large values for testing.
            _min = min > _MAXIMUM_ALLOWED_NUMBER_TO_RANDOMLY_GENERATE ? _MAXIMUM_ALLOWED_NUMBER_TO_RANDOMLY_GENERATE : (int)min;
            _max = max > _MAXIMUM_ALLOWED_NUMBER_TO_RANDOMLY_GENERATE ? _MAXIMUM_ALLOWED_NUMBER_TO_RANDOMLY_GENERATE : (int)max;

            _increment = increment;
            _random = random;
        }

        /// <summary>
        /// Changes the max allowed value - can only be done prior to number generation.
        /// </summary>
        /// <param name="value"></param>
        public void SetMaximumAllowedValue(int value)
        {
            if (_valuesHaveBeenGenerated)
            {
                throw new NotSupportedException(_NOT_SUPPORTED_OPERATION);
            }
            if (_min >= value)
            {
                _min = 0;
            }
            if (_max > value)
            {
                _max = value;
            }
        }

        /// <summary>
        /// Checks that a value occurs within the domain, taking into account the increment.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsWithinDomain(int value)
        {
            // If outside the bounds of the min and max
            if (value < _min || value > _max)
            {
                return false;
            }

            // Check if within the range and a valid value according to increment.
            long valueOffset = value - _min;

            if (valueOffset % _increment == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets values from the segment up to the quantity. 
        /// If less than quantity exist in the segment, the maximum amount for the segment is returned..
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IEnumerable<int> GetValues(int quantity)
        {
            _valuesHaveBeenGenerated = true;
            int quantityToRetrieve = MaxNumberOfValuesInSegment < quantity ? MaxNumberOfValuesInSegment : quantity;

            List<int> values = new List<int>();

            Func<int> func = null;
            if (SegmentValueOptions == RangeDomainSegmentOptions.Sequential)
            {
                func = GetNextSequentialValue;
            }
            else
            {
                func = GetNextRandomValue;
            }

            for (var i = 0; i < quantityToRetrieve; i++)
            {
                values.Add(func.Invoke());
            }

            return values;
        }

        private int GetNextSequentialValue()
        {
            int value;

            if (_returnedValues.Count == 0)
            {
                value = _min;
            }
            else
            {
                value = _returnedValues[_returnedValues.Count - 1] + _increment;
            }

            _returnedValues.Add(value);
            return value;
        }

        private int GetNextRandomValue()
        {
            int value = _random.GetRandomInt(_min, _max);

            // If the randomly generated value is not a valid value of the domain, or it's already been returned
            if (!IsWithinDomain(value) || _returnedValues.Contains(value))
            {
                // Find a value that falls within the domain
                // By adding and/or subtracting to the random value until encountering a value that
                // Is within the domain, and has not yet been used.
                for (int i = 1; i <= _increment * (_returnedValues.Count + 1); i++)
                {
                    int addToValue = value + i;
                    if (IsWithinDomain(addToValue) && !_returnedValues.Contains(addToValue))
                    {
                        _returnedValues.Add(addToValue);
                        return addToValue;
                    }

                    int subtractFromValue = value - i;
                    if (IsWithinDomain(subtractFromValue) && !_returnedValues.Contains(subtractFromValue))
                    {
                        _returnedValues.Add(subtractFromValue);
                        return subtractFromValue;
                    }
                }
            }

            _returnedValues.Add(value);
            return value;
        }
    }
}
