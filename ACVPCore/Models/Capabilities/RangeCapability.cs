using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace ACVPCore.Models.Capabilities
{
	public class RangeCapability : BaseCapability, IDomainSegment, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.Range;
		public long? Min { get; set; }
		public long? Max { get; set; }
		public int? Increment { get; set; }

		public long NumericSortValue => Min ?? Max ?? long.MaxValue;      //Sort by the min if not null, then max if min was null, then a garbage value if both are null

		public RangeCapability() { }

		public RangeCapability(string json)
		{
			//Deserialize the little chunk of JSON for the range
			var rangeContents = JsonSerializer.Deserialize<RangeDomainModel>(json);

			Min = rangeContents.Min;
			Max = rangeContents.Max;
			Increment = rangeContents.Increment;
		}

		public RangeCapability(Algorithms.DataTypes.Range range)
		{
			Min = range.Min;
			Max = range.Max;
			Increment = range.Increment;
		}


		public string AsString() => $"[{{{JsonSerializer.Serialize(new RangeDomainModel { Min = Min, Max = Max, Increment = Increment }, new JsonSerializerOptions { IgnoreNullValues = true })}}}]";

		public string AsDomainSegmentString() => $"{{{JsonSerializer.Serialize(new RangeDomainModel { Min = Min, Max = Max, Increment = Increment }, new JsonSerializerOptions { IgnoreNullValues = true })}}}";

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType && ((RangeCapability)capability).AsString() == AsString();	//bit of a kludge, but easier to just compare the string values...
		}

		private class RangeDomainModel
		{
			[JsonPropertyName("min")]
			public long? Min { get; set; }

			[JsonPropertyName("max")]
			public long? Max { get; set; }

			[JsonPropertyName("increment")]
			public int? Increment { get; set; }
		}

	}
}
