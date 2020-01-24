using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public class BooleanArrayCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.BooleanArray;
		public List<BooleanCapability> Values { get; set; } = new List<BooleanCapability>();

		public BooleanArrayCapability(IEnumerable<bool> values) => values.Select(x => new BooleanCapability { Value = x });

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType
				&& ((BooleanArrayCapability)capability).Values.Select(a => a.Value).OrderBy(a => a).SequenceEqual(Values.Select(b => b.Value).OrderBy(b => b));
		}
	}
}
