using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class RawCapability
	{
		public long ID { get; set; }
		public long AlgorithmPropertyID { get; set; }
		public long? ParentCapabilityID { get; set; }
		public bool HistoricalCapability { get; set; }
		public int CapabilityOrderIndex { get; set; }
		public bool? BooleanValue { get; set; }
		public string StringValue { get; set; }
		public long? NumberValue { get; set; }
		public string PropertyDisplayName { get; set; }
		public AlgorithmPropertyType PropertyType { get; set; }
		public int PropertyOrderIndex { get; set; }
		public bool HistoricalProperty { get; set; } = false;
		public bool IsRequired { get; set; } = false;
		public string UnitsLabel { get; set; }
		public bool HistoricalAlgorithm { get; set; }

		public bool IsHistorical => HistoricalAlgorithm || HistoricalProperty || HistoricalCapability;
	}
}
