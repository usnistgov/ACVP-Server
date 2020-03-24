using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class AES_CBC_CS3 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "payloadLen", Type = AlgorithmPropertyType.Domain)]
		public Domain PayloadLength { get; set; }

		public AES_CBC_CS3()
		{
			Name = "ACVP-AES-CBC-CS3";
			Revision = "1.0";
		}

		public AES_CBC_CS3(External.AES_CBC_CS3 external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
		}
	}
}
