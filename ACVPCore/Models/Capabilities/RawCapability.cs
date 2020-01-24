using System;
using System.Collections.Generic;
using System.Text;

namespace ACVPCore.Models.Capabilities
{
	public class RawCapability
	{
		public long ID { get; set; }
		public long? ParentCapabilityID { get; set; }
		public long? RootCapabilityID { get; set; }
		public long PropertyID { get; set; }
		public DatabaseCapabilityType CapabilityType { get; set; }
		public int Level { get; set; }
		public int OrderIndex { get; set; }
		public string StringValue { get; set; }
		public long? NumberValue { get; set; }
		public bool? BooleanValue { get; set; }		
	}
}
