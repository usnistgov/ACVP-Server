using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NIST.CVP.Math.Domain
{
    /// <summary>
    /// A domain of values - literal values and/or a range of values
    /// </summary>
    public class MathDomain
    {
        private readonly List<IDomainSegment> _domainSegments = new List<IDomainSegment>();
        public IEnumerable<IDomainSegment> DomainSegments => _domainSegments.AsReadOnly();

        /// <summary>
        /// Adds a domain segment to the <see cref="MathDomain"/>
        /// </summary>
        /// <param name="domainSegment">The domain segment to add</param>
        /// <returns>Returns itself for fluent API</returns>
        public MathDomain AddSegment(IDomainSegment domainSegment)
        {
            _domainSegments.Add(domainSegment);

            return this;
        }

        /// <summary>
        /// Sets get value option for all <see cref="IDomainSegment"/>s
        /// </summary>
        /// <param name="option">The option to set</param>
        /// <returns>Returns itself for fluent API</returns>
        public MathDomain SetRangeOptions(RangeDomainSegmentOptions option)
        {
            foreach (var domainSegment in _domainSegments)
            {
                domainSegment.SegmentValueOptions = option;
            }

            return this;
        }

        /// <summary>
        /// Allows for the setting of a new maximum allowed value, 
        /// as to create a pseudo "subset" from a full range.
        /// </summary>
        /// <param name="value">The value to set as the maximum value.</param>
        /// <returns>Returns itself for fluent API</returns>
        public MathDomain SetMaximumAllowedValue(int value)
        {
            foreach (var domainSegment in _domainSegments)
            {
                domainSegment.SetMaximumAllowedValue(value);
            }

            return this;
        }

        /// <summary>
        /// Gets the minimum and maximum for the entire <see cref="MathDomain"/> as an <see cref="IEnumerable{T}"/> of <see cref="int"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetDomainMinMaxAsEnumerable()
        {
            var rangeMinMax = GetDomainMinMax();
            List<int> minMax = new List<int>();

            minMax.Add(rangeMinMax.Minimum);

            // Add the max only if it's a different value than the minimum
            if (rangeMinMax.Minimum != rangeMinMax.Maximum)
            {
                minMax.Add(rangeMinMax.Maximum);
            }

            return minMax;
        }
        
        /// <summary>
        /// Gets the minimum and maximum for the entire <see cref="MathDomain"/> as <see cref="RangeMinMax"/>
        /// </summary>
        /// <returns></returns>
        public RangeMinMax GetDomainMinMax()
        {
            RangeMinMax minMax = null;

            foreach (var segment in DomainSegments)
            {
                var segmentMinMax = segment.RangeMinMax;

                if (minMax == null)
                {
                    minMax = new RangeMinMax()
                    {
                        Minimum = segmentMinMax.Minimum,
                        Maximum = segmentMinMax.Maximum
                    };

                    continue;
                }

                if (segmentMinMax.Minimum < minMax.Minimum)
                {
                    minMax.Minimum = segmentMinMax.Minimum;
                }

                if (segmentMinMax.Maximum > minMax.Maximum)
                {
                    minMax.Maximum = segmentMinMax.Maximum;
                }
            }

            return minMax;
        }

        /// <summary>
        /// Get the <see cref="IEnumerable{T}"/> of <see cref="RangeMinMax"/> 
        /// from each <see cref="IDomainSegment"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RangeMinMax> GetMinMaxRanges()
        {
            List<RangeMinMax> list = new List<RangeMinMax>();

            foreach (var segment in DomainSegments)
            {
                list.Add(segment.RangeMinMax);
            }

            return list;
        }

        /// <summary>
        /// Checks that a value exists within any <see cref="IDomainSegment"/>.
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <returns>true/false</returns>
        public bool IsWithinDomain(int value)
        {
            foreach (var segment in DomainSegments)
            {
                if (segment.IsWithinDomain(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the number of values specified from each segment (or up to the upper limit of the segment)
        /// </summary>
        /// <param name="numberOfValuesPerSegment">Max number of values to get per <see cref="IDomainSegment"/></param>
        /// <returns></returns>
        public IEnumerable<int> GetValues(int numberOfValuesPerSegment)
        {
            List<int> values = new List<int>();

            foreach (var domainSegment in _domainSegments)
            {
                values.AddRange(domainSegment.GetValues(numberOfValuesPerSegment));
            }

            return values
                .Distinct()
                .OrderBy(ob => ob);
        }

        public IEnumerable<int> GetValues(Func<int, bool> condition, int numberOfTotalValues, bool randomOrder)
        {
            var potentialOptions = new List<int>();

            foreach (var domainSegment in _domainSegments)
            {
                potentialOptions.AddRange(domainSegment.GetValues(condition, numberOfTotalValues));
            }

            if (randomOrder)
            {
                return potentialOptions.Distinct().OrderBy(a => Guid.NewGuid()).Take(numberOfTotalValues);
            }
            else
            {
                return potentialOptions.Distinct().OrderBy(a => a).Take(numberOfTotalValues);
            }
        }

        /// <summary>
        /// Gets the number of values specified from each <see cref="IDomainSegment"/> - 
        /// or up to the upper limit of the segment.
        ///
        /// Values are restricted to the <see cref="min"/> and <see cref="maxExclusive"/>
        /// </summary>
        /// <param name="min">The minimum value to return</param>
        /// <param name="max">The maximum value to return</param>
        /// <param name="numberOfValuesPerSegment">Max number of values to get per <see cref="IDomainSegment"/></param>
        /// <returns></returns>        
        public IEnumerable<int> GetValues(int min, int max, int numberOfValuesPerSegment)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} must be less than or equal to {nameof(max)}");
            }
            
            List<int> values = new List<int>();

            foreach (var domainSegment in _domainSegments)
            {
                values.AddRange(domainSegment.GetValues(min, max, numberOfValuesPerSegment));
            }

            return values
                .Distinct()
                .OrderBy(ob => ob);
        }

        public MathDomain GetDeepCopy()
        {
            var domain = new MathDomain();

            foreach(var segment in DomainSegments)
            {
                domain.AddSegment(segment.GetDeepCopy());
            }

            return domain;
        }
    }
}
