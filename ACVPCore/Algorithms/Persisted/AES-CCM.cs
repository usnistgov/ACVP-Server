using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CCM : PersistedAlgorithmBase
	{

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<long> KeyLength { get; set; }

		[AlgorithmProperty(Name = "tag", Type = AlgorithmPropertyType.NumberArray)]
		public List<long> TagLength { get; set; }

		[AlgorithmProperty(Name = "iv", Type = AlgorithmPropertyType.NumberArray)]
		public List<long> IVLength { get; set; }

		[AlgorithmProperty(Name = "pt", Type = AlgorithmPropertyType.Domain)]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty(Name = "aad", Type = AlgorithmPropertyType.Domain)]
		public Domain AADLength { get; set; }

		public AES_CCM()
		{
			Name = "ACVP-AES-CCM";
		}

		public AES_CCM(ACVPCore.Algorithms.External.AES_CCM external) : this()
		{
			KeyLength = external.KeyLength;
			TagLength = external.TagLength;
			IVLength = external.IVLength;
			PayloadLength = external.PayloadLength;
			AADLength = external.AADLength;
		}
	}
}
