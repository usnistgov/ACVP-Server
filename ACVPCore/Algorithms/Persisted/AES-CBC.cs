using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CBC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		public AES_CBC()
		{
			Name = "ACVP-AES-CBC";
			Revision = "1.0";
		}

		public AES_CBC(ACVPCore.Algorithms.External.AES_CBC external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
		}
	}
}
