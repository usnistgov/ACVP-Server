using System.Text.Json;
using Range = NIST.CVP.Libraries.Shared.Algorithms.DataTypes.Range;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class RangeCapability : BaseCapability
	{
		private string _jsonValue;
		public string JsonValue
		{
			get { return _jsonValue; }
			set
			{
				_jsonValue = value;
				var range = JsonSerializer.Deserialize<Range>(_jsonValue);
				Min = range.Min;
				Max = range.Max;
				Increment = range.Increment;
			}
		}

		protected long? Min { get; set; }
		protected long? Max { get; set; }
		protected int? Increment { get; set; }

		public override string ValueDisplayHTML
		{
			get
			{
				string innerHTML = "";

				if (Min != null && Max != null) innerHTML = $"{Min}-{Max}";
				else if (Min != null) innerHTML = $"Min {Min}";
				else if (Max != null) innerHTML = $"Max {Max}";

				if (Increment != null) innerHTML += $" Increment {Increment}";

				return (Historical) ? $"<s>{innerHTML}</s>" : $"{innerHTML}";
			}
		}

		public override string HTML
		{
			get
			{
				string innerHTML = "";

				if (Min != null && Max != null) innerHTML = $"{Min}-{Max}";
				else if (Min != null) innerHTML = $"Min {Min}";
				else if (Max != null) innerHTML = $"Max {Max}";

				if (Increment != null) innerHTML += $" Increment {Increment}";

				return (Historical) ? $"<s>{Label}: {innerHTML}{UnitsLabel}</s>" : $"{Label}: {innerHTML}{UnitsLabel}";
			}
		}

		public override long NumericSortValue => Min ?? Max ?? long.MaxValue;      //Sort by the min if not null, then max if min was null, then a garbage value if both are null

		public RangeCapability(RawCapability rawCapability)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			UnitsLabel = rawCapability.UnitsLabel;
			JsonValue = rawCapability.StringValue;
		}

		public RangeCapability(Range range)
		{
			Min = range.Min;
			Max = range.Max;
			Increment = range.Increment;
		}
	}
}
