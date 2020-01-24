using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public class CompositeArrayCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.CompositeArray;
		public List<CompositeCapability> ChildCapabilities { get; set; } = new List<CompositeCapability>();

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			if (capability.CapabilityType != CapabilityType) return false;

			//Get the children of the capability passed in, as will reference multiple times
			List<CompositeCapability> parameterChildCapabilities = ((CompositeArrayCapability)capability).ChildCapabilities;

			//Fail if the length of the child collections are different
			if (parameterChildCapabilities.Count != ChildCapabilities.Count) return false;

			//Since there is no way to order these, so no good way to do it other than iterating through one collection looking for a match in the other collection
			foreach (CompositeCapability compositeCapability in ChildCapabilities)
			{
				if (!parameterChildCapabilities.Any(x => compositeCapability.IsFunctionallyEquivalent(x))) return false;
			}

			//If we made it here they must match
			return true;
		}
	}
}
