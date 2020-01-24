using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;
using ACVPCore.Models.Capabilities;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CMAC : AlgorithmBase
	{
		[Property(Name = "capabilities", Type = DatabaseCapabilityType.Composite)]
		public List<CapabilityObject> Capabilities { get; set; } = new List<CapabilityObject>();

		public AES_CMAC()
		{
			Name = "CMAC-AES";
		}

		public AES_CMAC(ACVPCore.Algorithms.External.AES_CMAC external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new CapabilityObject
				{
					Direction = capability.Direction,
					KeyLength = capability.KeyLength,
					MacLength = capability.MacLength,
					MessageLength = capability.MessageLength
				});
			}
		}
	}

	public class CapabilityObject { 
		[Property(Name = "direction", Type = DatabaseCapabilityType.StringArray )]
		public List<string> Direction { get; set; }

		[Property(Name = "keyLen", Type = DatabaseCapabilityType.NumberArray)]
		public List<long> KeyLength { get; set; }

		[Property(Name = "mac", Type = DatabaseCapabilityType.Domain)]
		public Domain MacLength { get; set; }

		[Property(Name = "msg", Type = DatabaseCapabilityType.Domain)]
		public Domain MessageLength { get; set; }

		[Property(Name = "blockSize", Type = DatabaseCapabilityType.StringArray)]
		public List<string> BlockSize { get; set; }
	}
}
