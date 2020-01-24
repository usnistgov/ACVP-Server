using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models.Capabilities;
using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class CapabilityService : ICapabilityService
	{
		ICapabilityProvider _capabilityProvider;

		public CapabilityService(ICapabilityProvider capabilityProvider)
		{
			_capabilityProvider = capabilityProvider;
		}

		public List<ICapability> GetCapabilitiesForComparison(long scenarioAlgorithmId)
		{
			//Get the raw capabilities
			List<RawCapability> rawCapabilities = _capabilityProvider.GetRawCapabilitiesForScenarioAlgorithm(scenarioAlgorithmId);

			//Convert from generic raw capabilities to typed capabilities
			List<ICapability> comparableCapabilities = new List<ICapability>();

			//Iterate over the root level capabilities
			foreach (RawCapability rawCapability in rawCapabilities.Where(c => c.Level == 0))
			{
				comparableCapabilities.Add(BuildCapabilityForComparison(rawCapability, rawCapabilities));
			}

			return comparableCapabilities;
		}


		private ICapability BuildCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> rawCapabilities) => rawCapability.CapabilityType switch
		{
			DatabaseCapabilityType.Boolean => BuildBooleanCapabilityForComparison(rawCapability),
			DatabaseCapabilityType.BooleanArray => BuildBooleanArrayCapabilityForComparison(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
			DatabaseCapabilityType.Composite => BuildCompositeCapabilityForComparison(rawCapability, rawCapabilities),
			DatabaseCapabilityType.CompositeArray => BuildCompositeArrayCapabilityForComparison(rawCapability, rawCapabilities),
			DatabaseCapabilityType.Domain => BuildDomainCapabilityForComparison(rawCapability),
			DatabaseCapabilityType.Long => BuildNumericCapabilityForComparison(rawCapability),
			DatabaseCapabilityType.LongArray => BuildNumericArrayCapabilityForComparison(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
			DatabaseCapabilityType.Number => BuildNumericCapabilityForComparison(rawCapability),
			DatabaseCapabilityType.NumberArray => BuildNumericArrayCapabilityForComparison(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
			DatabaseCapabilityType.Range => BuildRangeCapabilityForComparison(rawCapability),
			DatabaseCapabilityType.RangeArray => BuildRangeArrayCapabilityForComparison(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
			DatabaseCapabilityType.String => BuildStringCapabilityForComparison(rawCapability),
			DatabaseCapabilityType.StringArray => BuildStringArrayCapabilityForComparison(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID))
		};

		private BooleanCapability BuildBooleanCapabilityForComparison(RawCapability rawCapability) => new BooleanCapability
		{
			Value = (bool)rawCapability.BooleanValue
		};

		private BooleanArrayCapability BuildBooleanArrayCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities) => new BooleanArrayCapability
		{
			Values = childCapabilities.OrderBy(c => c.OrderIndex).Select(c => BuildBooleanCapabilityForComparison(c)).ToList()
		};

		private CompositeCapability BuildCompositeCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> rawCapabilities) => new CompositeCapability
		{
			ChildCapabilities = rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID).Select(c => BuildCapabilityForComparison(c, rawCapabilities)).ToList()
		};

		private CompositeArrayCapability BuildCompositeArrayCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> rawCapabilities) => new CompositeArrayCapability
		{
			ChildCapabilities = rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID).Select(c => BuildCompositeCapabilityForComparison(c, rawCapabilities)).ToList()
		};

		private DomainCapability BuildDomainCapabilityForComparison(RawCapability rawCapability) => new DomainCapability(rawCapability.StringValue);

		private NumericCapability BuildNumericCapabilityForComparison(RawCapability rawCapability) => new NumericCapability { Value = (long)rawCapability.NumberValue };

		private NumericArrayCapability BuildNumericArrayCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities) => new NumericArrayCapability
		{
			Values = childCapabilities.OrderBy(c => c.OrderIndex).Select(c => BuildNumericCapabilityForComparison(c)).ToList()
		};

		private RangeCapability BuildRangeCapabilityForComparison(RawCapability rawCapability) => new RangeCapability(rawCapability.StringValue);

		private RangeArrayCapability BuildRangeArrayCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities) => new RangeArrayCapability
		{
			ChildCapabilities = childCapabilities.OrderBy(c => c.OrderIndex).Select(c => BuildRangeCapabilityForComparison(c))
		};

		private StringCapability BuildStringCapabilityForComparison(RawCapability rawCapability) => new StringCapability { Value = rawCapability.StringValue };

		private StringArrayCapability BuildStringArrayCapabilityForComparison(RawCapability rawCapability, IEnumerable<RawCapability> childCapabilities) => new StringArrayCapability
		{
			Values = childCapabilities.OrderBy(c => c.OrderIndex).Select(c => BuildStringCapabilityForComparison(c)).ToList()
		};
	}
}
