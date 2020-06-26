using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using Range = NIST.CVP.Libraries.Shared.Algorithms.DataTypes.Range;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class DomainCapability : BaseCapability
	{
		private IEnumerable<BaseCapability> Segments { get; set; }
		private List<BaseCapability> SortedSegments
		{
			get
			{
				return Segments.OrderBy(x => x.NumericSortValue).ToList();
			}
		}

		public string JsonValue { get; set; }

		public override string HTML
		{
			get
			{
				Domain domain = JsonSerializer.Deserialize<Domain>(JsonValue);

				List<BaseCapability> segments = new List<BaseCapability>();
				foreach (IDomainSegment segment in domain.Segments)
				{
					if (segment is NumericSegment)
					{
						segments.Add(new NumericCapability(((NumericSegment)segment).Value));
					}
					else
					{
						segments.Add(new RangeCapability((Range)segment));
					}
				}
				Segments = segments;

				string innerHTML = String.Join(", ", SortedSegments.Select(v => v.ValueDisplayHTML));
				return (Historical) ? $"<s>{Label}: {innerHTML}{UnitsLabel}</s>" : $"{Label}: {innerHTML}{UnitsLabel}";
			}
		}

		public DomainCapability(RawCapability rawCapability)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			UnitsLabel = rawCapability.UnitsLabel;
			JsonValue = rawCapability.StringValue;
		}
	}
}
