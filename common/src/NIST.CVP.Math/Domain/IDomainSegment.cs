using System.Collections.Generic;

namespace NIST.CVP.Math.Domain
{
    /// <summary>
    /// Describes a DomainSegment - a portion of a Domain.
    /// </summary>
    public interface IDomainSegment
    {
        /// <summary>
        /// Get the min/max of the segment
        /// </summary>
        RangeMinMax RangeMinMax { get; }
        /// <summary>
        /// Sets options for obtaining values from the segment.
        /// </summary>
        RangeDomainSegmentOptions SegmentValueOptions { set; }
        /// <summary>
        /// Allows for the setting of a new maximum allowed value, 
        /// as to create a pseudo "subset" from a full range.
        /// </summary>
        /// <param name="value"></param>
        void SetMaximumAllowedValue(int value);
        /// <summary>
        /// Does the provided value occur within the domain segment?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsWithinDomain(int value);
        /// <summary>
        /// Get values from the segment, up to the quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        IEnumerable<int> GetValues(int quantity);
    }
}