using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public abstract class BaseCapability
	{
		public abstract DatabaseCapabilityType CapabilityType { get; }
		public bool Historical { get; set; } = false;
		public int Level { get; set; }
		public int? OrderIndex { get; set; }
		//public virtual long NumericSortValue { get; set; }

		public long? ID { get; set; }
		public long? ParentID { get; set; }
		public long? RootID { get; set; }
		public long PropertyID { get; set; }

		public ACVPCapabilityType ACVPCapabilityType => CapabilityType switch
		{
			DatabaseCapabilityType.Boolean => Capabilities.ACVPCapabilityType.Primitive,
			DatabaseCapabilityType.BooleanArray => Capabilities.ACVPCapabilityType.Array,
			DatabaseCapabilityType.Composite => Capabilities.ACVPCapabilityType.Composite,
			DatabaseCapabilityType.CompositeArray => Capabilities.ACVPCapabilityType.Array,
			DatabaseCapabilityType.Domain => Capabilities.ACVPCapabilityType.Object,
			DatabaseCapabilityType.Long => Capabilities.ACVPCapabilityType.Primitive,
			DatabaseCapabilityType.LongArray => Capabilities.ACVPCapabilityType.Array,
			DatabaseCapabilityType.Number => Capabilities.ACVPCapabilityType.Primitive,
			DatabaseCapabilityType.NumberArray => Capabilities.ACVPCapabilityType.Array,
			DatabaseCapabilityType.Range => Capabilities.ACVPCapabilityType.Object,
			DatabaseCapabilityType.RangeArray => Capabilities.ACVPCapabilityType.Array,
			DatabaseCapabilityType.String => Capabilities.ACVPCapabilityType.Primitive,
			DatabaseCapabilityType.StringArray => Capabilities.ACVPCapabilityType.Array,
			_ => Capabilities.ACVPCapabilityType.Object		//This one is garbage, just trying to avoid a warning
		};

		public abstract bool IsFunctionallyEquivalent(BaseCapability capability);
	}
}
