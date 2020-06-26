using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public static class CapabilityFactory
	{
		public static BaseCapability BuildCapability(RawCapability rawCapability, IEnumerable<RawCapability> rawCapabilities) =>
			rawCapability.PropertyType switch
			{
				AlgorithmPropertyType.Boolean => new BooleanCapability(rawCapability),
				AlgorithmPropertyType.BooleanCollection => new BooleanCollectionCapability(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
				AlgorithmPropertyType.Composite => new CompositeCapability(rawCapability, rawCapabilities),                             //Since the capabilities are nested this needs more than just the immediate children, so just pass everything rather than trying to find all descendants
				AlgorithmPropertyType.CompositeCollection => new CompositeCollectionCapability(rawCapability, rawCapabilities),         //Since the capabilities are nested this needs more than just the immediate children, so just pass everything rather than trying to find all descendants
				AlgorithmPropertyType.Domain => new DomainCapability(rawCapability),
				AlgorithmPropertyType.Integer => new NumericCapability(rawCapability),
				AlgorithmPropertyType.IntegerCollection => new NumericCollectionCapability(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
				AlgorithmPropertyType.Long => new NumericCapability(rawCapability),
				AlgorithmPropertyType.LongCollection => new NumericCollectionCapability(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
				AlgorithmPropertyType.Range => new RangeCapability(rawCapability),
				AlgorithmPropertyType.RangeCollection => new RangeCollectionCapability(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
				AlgorithmPropertyType.String => new StringCapability(rawCapability),
				AlgorithmPropertyType.StringCollection => new StringCollectionCapability(rawCapability, rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID)),
				_ => null,
			};
	}
}
