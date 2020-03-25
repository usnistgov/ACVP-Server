using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class AES_CFB1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		public AES_CFB1()
		{
			Name = "ACVP-AES-CFB1";
			Revision = "1.0";
		}

		public AES_CFB1(External.AES_CFB1 external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
		}
	}
}
