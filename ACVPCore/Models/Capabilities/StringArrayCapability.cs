using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ACVPCore.Models.Capabilities
{
	public class StringArrayCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.StringArray;
		public List<StringCapability> Values { get; set; } = new List<StringCapability>();

		public StringArrayCapability() { }
		public StringArrayCapability(IEnumerable<string> values)
		{
			//Skip any null values
			Values = values.Where(x => x != null).Select(x => new StringCapability { Value = x }).ToList();
		}

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType
				&& ((StringArrayCapability)capability).Values.Select(a => a.Value).OrderBy(a => a).SequenceEqual(Values.Select(b => b.Value).OrderBy(b => b));
		}
	}
}
