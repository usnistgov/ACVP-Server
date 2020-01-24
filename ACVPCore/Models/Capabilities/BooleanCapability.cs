using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public class BooleanCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.Boolean;
		public bool Value { get; set; }
		public bool Required { get; set; }

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType && ((BooleanCapability)capability).Value == Value;
		}
	}
}
