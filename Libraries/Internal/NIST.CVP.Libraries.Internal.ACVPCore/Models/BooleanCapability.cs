namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class BooleanCapability : BaseCapability
	{
		public bool Value { get; set; }
		public bool Required { get; set; }

		public override string HTML
		{
			get
			{
				string html;

				if (Required)
				{
					html = HistoricalizeHTML($"{Label}: {ValueText}");
				}
				else
				{
					html = Value ? HistoricalizeHTML(Label) : "";
				}

				return html;        //HistoricalizeHTML has to be called where it is because don't want to call it when property not required, value is false, and property is historical - would return a useless <s></s> to the caller
			}
		}

		public override string ValueDisplayHTML => Historical ? $"<s>{ValueText}</s>" : ValueText;

		private string ValueText => Value ? "Yes" : "No";

		public BooleanCapability(RawCapability rawCapability)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			Required = rawCapability.IsRequired;
			Value = (bool)rawCapability.BooleanValue;
		}

		private string HistoricalizeHTML(string html) => (Historical) ? $"<s>{html}</s>" : html;
	}
}
