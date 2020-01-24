namespace ACVPCore.Models.Capabilities
{
	public class NumericCapability : BaseCapability, IDomainSegment, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.Number;		//Database model has long and int (number) as separate types... Not sure if that will come back to bite me
		public long Value { get; set; }
		public long NumericSortValue => Value;

		public string AsString() => Value.ToString();
		public string AsDomainSegmentString() => Value.ToString();

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType && ((NumericCapability)capability).Value == Value;
		}

		public long Persist()
		{
			//This needs stuff... ValidationService...

		}
	}
}
