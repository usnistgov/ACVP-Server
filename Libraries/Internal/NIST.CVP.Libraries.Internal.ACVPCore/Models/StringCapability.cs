namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class StringCapability : BaseCapability
	{
		public string Value { get; set; }

		public override string HTML => (Historical) ? $"<s>{Label}: {Value}</s>" : $"{Label}: {Value}";

		public override string ValueDisplayHTML => (Historical) ? $"<s>{Value}</s>" : Value;

		public StringCapability(RawCapability rawCapability)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			Value = rawCapability.StringValue;
		}
	}
}
