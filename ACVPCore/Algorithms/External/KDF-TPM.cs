namespace ACVPCore.Algorithms.External
{
	public class KDF_TPM : AlgorithmBase, IExternalAlgorithm
	{
		public KDF_TPM()
		{
			Name = "kdf-components";
			Mode = "tpm";
			Revision = "1.0";
		}
	}
}
