using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class BooleanCollectionCapability : BaseCapability
	{
		public IEnumerable<BooleanCapability> Values { get; set; }

		public override string HTML
		{
			get
			{
				string valuesHTML = String.Join(", ", Values.OrderBy(x => x.Value ? 1 : 2).Select(v => v.ValueDisplayHTML));        //The logic in the order by makes sure the positive value comes first, negative second
				return (Historical) ? $"<s>{Label}: {valuesHTML}{UnitsLabel}</s>" : $"{Label}: {valuesHTML}{UnitsLabel}";
			}
		}

		public BooleanCollectionCapability(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			Values = childCapabilities.OrderBy(c => c.CapabilityOrderIndex).Select(c => new BooleanCapability(c));
		}
	}
}
