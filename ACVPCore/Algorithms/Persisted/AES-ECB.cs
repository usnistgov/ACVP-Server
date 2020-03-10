using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_ECB : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "pt", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> PayloadLength { get; set; }

		public AES_ECB()
		{
			Name = "ACVP-AES-ECB";
			Revision = "1.0";
		}

		public AES_ECB(ACVPCore.Algorithms.External.AES_ECB external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
		}
	}
}
