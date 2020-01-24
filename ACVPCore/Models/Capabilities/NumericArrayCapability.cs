using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public class NumericArrayCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.NumberArray;

		public List<NumericCapability> Values { get; set; } = new List<NumericCapability>();

		public NumericArrayCapability(IEnumerable<long> values)
		{
			Values = values.Select(x => new NumericCapability { Value = x }).ToList();
		}

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType
				&& ((NumericArrayCapability)capability).Values.Select(a => a.Value).OrderBy(a => a).SequenceEqual(Values.Select(b => b.Value).OrderBy(b => b));
		}
	}
}
