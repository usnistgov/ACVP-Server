using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Models.Capabilities
{
	public class RangeArrayCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.RangeArray;
		public List<RangeCapability> ChildCapabilities { get; set; } = new List<RangeCapability>();

		public RangeArrayCapability() { }
		public RangeArrayCapability(IEnumerable<Algorithms.DataTypes.Range> values)
		{
			ChildCapabilities = values.Where(x => x != null).Select(x => new RangeCapability(x)).ToList();
		}

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType
				&& ((RangeArrayCapability)capability).ChildCapabilities.Select(a => a.AsString()).OrderBy(a => a).SequenceEqual(ChildCapabilities.Select(b => b.AsString()).OrderBy(b => b));	//Kludgy, but reducing it to a comparison of lists of strings
		}
	}
}
