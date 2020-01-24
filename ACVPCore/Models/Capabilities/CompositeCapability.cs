using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ACVPCore.Models.Capabilities
{
	public class CompositeCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.Composite;
		public List<BaseCapability> ChildCapabilities { get; set; } = new List<BaseCapability>();

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			if (capability.CapabilityType != CapabilityType) return false;

			//Sort the child capabilities based on their propery IDs, as that's what we'll compare based on
			var orderedChildCapabilities = ChildCapabilities.OrderBy(x => x.PropertyID).ToList();
			var orderedParameterChildCapabilities = ((CompositeCapability)capability).ChildCapabilities.OrderBy(x => x.PropertyID).ToList();

			//Both collections must have the same properties - this will check the length and the values
			if (!orderedChildCapabilities.Select(a => a.PropertyID).SequenceEqual(orderedParameterChildCapabilities.Select(b => b.PropertyID))) return false;

			//Know that they use the same properties, so loop through them and see if they match. Fail out as soon as a mismatch is found
			for (int i = 0; i < orderedChildCapabilities.Count(); i++)
			{
				if (!orderedChildCapabilities[i].IsFunctionallyEquivalent(orderedParameterChildCapabilities[i])) return false;
			}

			//Must match if we made it here
			return true;
		}
	}
}
