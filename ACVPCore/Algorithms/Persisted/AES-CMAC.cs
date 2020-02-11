using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CMAC : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public AES_CMAC()
		{
			Name = "CMAC-AES";
			Revision = "1.0";
		}

		public AES_CMAC(ACVPCore.Algorithms.External.AES_CMAC external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					Direction = capability.Direction,
					KeyLength = capability.KeyLength,
					MacLength = capability.MacLength,
					MessageLength = capability.MessageLength
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty(Name = "direction")]
			public List<string> Direction { get; set; }

			[AlgorithmProperty(Name = "keyLen")]
			public List<int> KeyLength { get; set; }

			[AlgorithmProperty(Name = "mac")]
			public Domain MacLength { get; set; }

			[AlgorithmProperty(Name = "msg")]
			public Domain MessageLength { get; set; }

			[AlgorithmProperty(Name = "blockSize")]
			public List<string> BlockSize { get; set; }
		}
	}

	
}
