using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class AES_CTR : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "payloadLen", Type = AlgorithmPropertyType.Domain)]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty(Name = "overflowCounter", Type = AlgorithmPropertyType.Boolean)]
		public bool? OverflowCounter { get; set; }

		[AlgorithmProperty(Name = "incrementalCounter", Type = AlgorithmPropertyType.Boolean)]
		public bool? IncrementalCounter { get; set; }

		[AlgorithmProperty(Name = "ctrSource", Type = AlgorithmPropertyType.StringArray)]
		public List<string> CounterSource { get; set; }

		public AES_CTR()
		{
			Name = "ACVP-AES-CTR";
			Revision = "1.0";
		}

		public AES_CTR(External.AES_CTR external) : this()
		{
			Direction = external.Direction;
			KeyLength = external.KeyLength;
			PayloadLength = external.PayloadLength;
			OverflowCounter = external.OverflowCounter;
			IncrementalCounter = external.IncrementalCounter;
		}
	}
}
