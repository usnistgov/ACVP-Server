using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class TDES_CMAC : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<CapabilityObject> Capabilities { get; set; } = new List<CapabilityObject>();

		public TDES_CMAC()
		{
			Name = "CMAC-TDES";
			Revision = "1.0";
		}

		public class CapabilityObject
		{
			[AlgorithmProperty(Name = "direction")]
			public List<string> Direction { get; set; }

			[AlgorithmProperty(Name = "key")]
			public List<int> KeyingOption { get; set; }

			[AlgorithmProperty(Name = "mac")]
			public Domain MacLength { get; set; }

			[AlgorithmProperty(Name = "msg")]
			public Domain MessageLength { get; set; }

			[AlgorithmProperty(Name = "blockSize")]
			public List<string> BlockSize { get; set; }
		}

		public TDES_CMAC(External.TDES_CMAC external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new CapabilityObject
				{
					Direction = capability.Direction,
					KeyingOption = capability.KeyingOption,
					MacLength = capability.MacLength,
					MessageLength = capability.MessageLength
					//BlockSize not used
				});
			}
		}
	}

	
}
