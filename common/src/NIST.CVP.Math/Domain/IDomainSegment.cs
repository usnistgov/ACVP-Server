using System.Collections.Generic;

namespace NIST.CVP.Math.Domain
{
    /// <summary>
    /// Describes a DomainSegment - a portion of a Domain.
    /// </summary>
    public interface IDomainSegment
    {
        /// <summary>
        /// Get the min/max of the <see cref="IDomainSegment"/>
        /// </summary>
        RangeMinMax RangeMinMax { get; }
        /// <summary>
        /// Sets options for obtaining values from the <see cref="IDomainSegment"/>.
        /// </summary>
        RangeDomainSegmentOptions SegmentValueOptions { set; }
        /// <summary>
        /// Allows for the setting of a new maximum allowed value, 
        /// as to create a pseudo "subset" from a full range.
        /// </summary>
        /// <param name="value"></param>
        void SetMaximumAllowedValue(int value);
        /// <summary>
        /// Does the provided value occur within the <see cref="IDomainSegment"/>?
        /// </summary>
        /// <param name="value">The value to look for within the <see cref="IDomainSegment"/></param>
        /// <returns></returns>
        bool IsWithinDomain(int value);
        /// <summary>
        /// Get values from the <see cref="IDomainSegment"/>, up to the quantity
        /// </summary>
        /// <param name="quantity">The maximum number of values to return from the <see cref="IDomainSegment"/></param>
        /// <returns></returns>
        IEnumerable<int> GetValues(int quantity);
        /// <summary>
        /// Get values from the <see cref="IDomainSegment"/>, with a minimum of <see cref="min"/>
        /// and a maximum <see cref="max"/>, up to the quantity
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <param name="quantity">The maximum number of values to return from the <see cref="IDomainSegment"/></param>
        /// <returns></returns>
        IEnumerable<int> GetValues(int min, int max, int quantity);
        /// <summary>
        /// Returns a deep copy of the <see cref="IDomainSegment"/>
        /// </summary>
        /// <returns></returns>
        IDomainSegment GetDeepCopy();
    }
}