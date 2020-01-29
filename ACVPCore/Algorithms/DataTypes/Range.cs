using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.DataTypes
{
	public class Range : IDomainSegment
	{
		[JsonPropertyName("min")]
		public long? Min { get; set; }

		[JsonPropertyName("max")]
		public long? Max { get; set; }

		[JsonPropertyName("increment")]
		public int? Increment { get; set; }
	}
}
