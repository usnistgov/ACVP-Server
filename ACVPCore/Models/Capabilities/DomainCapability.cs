using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Models.Capabilities
{
	public class DomainCapability : BaseCapability, ICapability
	{
		public override DatabaseCapabilityType CapabilityType => DatabaseCapabilityType.Domain;
		public List<IDomainSegment> Segments { get; set; } = new List<IDomainSegment>();

		public List<IDomainSegment> SortedSegments => Segments.OrderBy(x => x.NumericSortValue).ToList();

		public string ValueJson => $"[{string.Join(",", SortedSegments.Select(x => x.AsDomainSegmentString()))}]";

		public DomainCapability(string json)
		{
			var stringSegments = json.Split(",");

			foreach (string stringSegment in stringSegments)
			{
				if (long.TryParse(stringSegment, out long numberValue))
				{
					Segments.Add(new NumericCapability { Value = numberValue });
				}
				else
				{
					Segments.Add(new RangeCapability(stringSegment));
				}
			}
		}

		public DomainCapability(Domain domain)
		{
			//Create an add the appropriate type of capability for each segment in the parameter domain
			foreach (var segment in domain.Segments)
			{
				if (segment is NumericSegment)
					Segments.Add(new NumericCapability
					{
						Value = ((NumericSegment)segment).Value
					});
				else
				{
					Range range = (Range)segment;
					Segments.Add(new RangeCapability
					{
						Min = range.Min,
						Max = range.Max,
						Increment = range.Increment
					});
				}
			}
		}

		public override bool IsFunctionallyEquivalent(BaseCapability capability)
		{
			return capability.CapabilityType == CapabilityType
				&& ((DomainCapability)capability).ValueJson == ValueJson;   //A kludge, but easier to compare strings than to break it down
		}
	}
}
