using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public class StringCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.String;
		public string Value { get; set; }

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType && ((StringCapability)capability).Value == Value;
		}
	}
}
