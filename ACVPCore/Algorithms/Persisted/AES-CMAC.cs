using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CMAC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "capabilities", Type = AlgorithmPropertyType.Composite)]
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
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray )]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "keyLen", Type = AlgorithmPropertyType.NumberArray)]
		public List<long> KeyLength { get; set; }

		[AlgorithmProperty(Name = "mac", Type = AlgorithmPropertyType.Domain)]
		public Domain MacLength { get; set; }

		[AlgorithmProperty(Name = "msg", Type = AlgorithmPropertyType.Domain)]
		public Domain MessageLength { get; set; }

		[AlgorithmProperty(Name = "blockSize", Type = AlgorithmPropertyType.StringArray)]
		public List<string> BlockSize { get; set; }
	}
}
