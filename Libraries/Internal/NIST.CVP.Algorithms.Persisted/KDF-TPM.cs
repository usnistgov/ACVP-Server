﻿using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class KDF_TPM : PersistedAlgorithmBase
	{
		public KDF_TPM()
		{
			Name = "kdf-components";
			Mode = "tpm";
			Revision = "1.0";
		}

		public KDF_TPM(External.KDF_TPM external) : this()
		{
		}
	}

	
}