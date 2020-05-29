using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class KDF_ANSIX942 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("kdfType")]
		public List<string> KDFTypes { get; set; }

		[AlgorithmProperty("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		[AlgorithmProperty("otherInfoLen")]
		public Domain OtherInfoLength { get; set; }

		[AlgorithmProperty("zzLen")]
		public Domain ZZLength { get; set; }

		[AlgorithmProperty("keyLen")]
		public Domain KeyLength { get; set; }

		public KDF_ANSIX942()
		{
			Name = "kdf-components";
			Mode = "ansix9.42";
			Revision = "1.0";
		}

		public KDF_ANSIX942(External.KDF_ANSIX942 external) : this()
		{
			KDFTypes = external.KDFTypes;
			HashAlgorithms = external.HashAlgorithms;
			OtherInfoLength = external.OtherInfoLength;
			ZZLength = external.ZZLength;
			KeyLength = external.KeyLength;
		}
	}
}
