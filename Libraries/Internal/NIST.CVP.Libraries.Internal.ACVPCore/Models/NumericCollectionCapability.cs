using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class NumericCollectionCapability : BaseCapability
	{
		public IEnumerable<NumericCapability> Values { get; set; }

		public override string HTML
		{
			get
			{
				string valuesHTML = String.Join(", ", Values.OrderBy(x => x.Value).Select(v => v.ValueDisplayHTML));
				return (Historical) ? $"<s>{Label}: {valuesHTML}{UnitsLabel}</s>" : $"{Label}: {valuesHTML}{UnitsLabel}";
			}
		}

		public NumericCollectionCapability(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			UnitsLabel = rawCapability.UnitsLabel;
			Values = childCapabilities.OrderBy(c => c.CapabilityOrderIndex).Select(c => new NumericCapability(c));
		}
	}
}
