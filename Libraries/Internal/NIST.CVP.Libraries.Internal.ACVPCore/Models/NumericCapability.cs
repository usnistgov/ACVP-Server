namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class NumericCapability : BaseCapability
	{
		public long Value { get; set; }

		public override long NumericSortValue => Value;

		public override string HTML =>(Historical) ? $"<s>{Label}: {Value}{UnitsLabel}</s>" : $"{Label}: {Value}{UnitsLabel}";

		public override string ValueDisplayHTML => (Historical) ? $"<s>{Value}</s>" : Value.ToString();

		public NumericCapability(RawCapability rawCapability)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			UnitsLabel = rawCapability.UnitsLabel;
			Value = (long)rawCapability.NumberValue;
		}

		public NumericCapability(long value)
		{
			Value = value;
		}
	}
}
