using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class AES_GCM : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "ivgen", Type = AlgorithmPropertyType.String)]
		public string IVGen { get; set; }

		[AlgorithmProperty(Name = "ivgenmode", Type = AlgorithmPropertyType.String)]
		public string IVGenMode { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "tag", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> TagLength { get; set; }

		[AlgorithmProperty(Name = "iv", Type = AlgorithmPropertyType.Domain)]
		public Domain IVLength { get; set; }

		[AlgorithmProperty(Name = "pt", Type = AlgorithmPropertyType.Domain)]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty(Name = "aad", Type = AlgorithmPropertyType.Domain)]
		public Domain AADLength { get; set; }

		public AES_GCM()
		{
			Name = "ACVP-AES-GCM";
			Revision = "1.0";
		}

		public AES_GCM(External.AES_GCM external) : this()
		{
			Direction = external.Direction;
			IVGen = external.IVGen;
			IVGenMode = external.IVGenMode;
			KeyLength = external.KeyLength;
			TagLength = external.TagLength;
			IVLength = external.IVLength;
			PayloadLength = external.PayloadLength;
			AADLength = external.AADLength;
		}
	}
}
