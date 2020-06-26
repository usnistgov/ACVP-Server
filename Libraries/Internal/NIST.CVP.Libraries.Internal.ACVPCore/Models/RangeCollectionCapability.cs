using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class RangeCollectionCapability : BaseCapability
	{
		public IEnumerable<RangeCapability> ChildCapabilities { get; set; }

		private IEnumerable<RangeCapability> SortedSegments => ChildCapabilities.OrderBy(x => x.NumericSortValue);

		public override string HTML
		{
			get
			{
				string innerHTML = String.Join(", ", SortedSegments.Select(v => v.ValueDisplayHTML));
				return (Historical) ? $"<s>{Label}: {innerHTML}{UnitsLabel}</s>" : $"{Label}: {innerHTML}{UnitsLabel}";
			}
		}

		public RangeCollectionCapability(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			UnitsLabel = rawCapability.UnitsLabel;
			ChildCapabilities = childCapabilities.OrderBy(c => c.CapabilityOrderIndex).Select(c => new RangeCapability(c));
		}
	}
}
