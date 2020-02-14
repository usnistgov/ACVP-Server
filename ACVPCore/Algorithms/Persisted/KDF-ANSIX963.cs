using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class KDF_ANSIX963 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("hashAlg")]
		public List<string> HashAlgorithms { get; set; }

		[AlgorithmProperty("fieldSize")]
		public List<int> FieldSizes { get; set; }

		[AlgorithmProperty("sharedInfoLength")]
		public Domain SharedInfoLength { get; set; }

		[AlgorithmProperty("keyDataLength")]
		public Domain KeyDataLength { get; set; }

		public KDF_ANSIX963()
		{
			Name = "kdf-components";
			Mode = "ansix9.63";
			Revision = "1.0";
		}

		public KDF_ANSIX963(ACVPCore.Algorithms.External.KDF_ANSIX963 external) : this()
		{
			HashAlgorithms = external.HashAlgorithms;
			FieldSizes = external.FieldSizes;
			SharedInfoLength = external.SharedInfoLength;
			KeyDataLength = external.KeyDataLength;
		}
	}
}
