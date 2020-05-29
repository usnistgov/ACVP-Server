using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class AES_CBC : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "pt", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> PayloadLength { get; set; }

		public AES_CBC()
		{
			Name = "ACVP-AES-CBC";
			Revision = "1.0";
		}

		public AES_CBC(External.AES_CBC external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
		}
	}
}
