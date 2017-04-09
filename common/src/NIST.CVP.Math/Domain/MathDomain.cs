using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Adds a domain segment to the Domain
        /// </summary>
        /// <param name="domainSegment">The domain segment to add</param>
        public void AddSegment(IDomainSegment domainSegment)
        {
            _domainSegments.Add(domainSegment);
        }

        /// <summary>
        /// Sets get value option for the domain segments
        /// </summary>
        /// <param name="option">The option to set</param>
        public void SetRangeOptions(RangeDomainSegmentOptions option)
        {
            foreach (var domainSegment in _domainSegments)
            {
                domainSegment.SegmentValueOptions = option;
            }
        }

        /// <summary>
        /// Allows for the setting of a new maximum allowed value, 
        /// as to create a pseudo "subset" from a full range.
        /// </summary>
        /// <param name="value"></param>
        public void SetMaximumAllowedValue(int value)
        {
            foreach (var domainSegment in _domainSegments)
            {
                domainSegment.SetMaximumAllowedValue(value);
            }
        }

        /// <summary>
        /// Gets the minimum and maximum for the entire domain
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
        /// Checks that a value exists within a domain segment
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
        /// <param name="numberOfValuesPerSegment"></param>
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
    }
}
