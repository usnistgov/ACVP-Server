using Newtonsoft.Json;

namespace LCAVPCore.Registration.MathDomain
{
	public class Range
	{
		[JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
		public int? Min { get; set; }

		[JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
		public int? Max { get; set; }

		[JsonProperty("increment", NullValueHandling = NullValueHandling.Ignore)]
		public int? Increment { get; set; }

		public ACVPCore.Algorithms.DataTypes.Range ToCoreRange() => new ACVPCore.Algorithms.DataTypes.Range { Min = Min, Max = Max, Increment = Increment };
	}
}
