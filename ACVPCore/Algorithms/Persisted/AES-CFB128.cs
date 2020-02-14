using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CFB128 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		public AES_CFB128()
		{
			Name = "ACVP-AES-CFB128";
			Revision = "1.0";
		}

		public AES_CFB128(ACVPCore.Algorithms.External.AES_CFB128 external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
		}
	}
}
