using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class StringCollectionCapability : BaseCapability
	{
		public IEnumerable<StringCapability> Values { get; set; }

		public override string HTML
		{
			get
			{
				string valuesHTML = String.Join(", ", Values.OrderBy(x => x.Value).Select(v => v.ValueDisplayHTML));
				return (Historical) ? $"<s>{Label}: {valuesHTML}</s>" : $"{Label}: {valuesHTML}";
			}
		}

		public StringCollectionCapability(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			Values = childCapabilities.OrderBy(c => c.CapabilityOrderIndex).Select(c => new StringCapability(c));
		}
	}
}
