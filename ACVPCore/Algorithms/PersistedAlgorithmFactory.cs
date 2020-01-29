using ACVPCore.Algorithms.External;
using ACVPCore.Algorithms.Persisted;

namespace ACVPCore.Algorithms
{
	public static class PersistedAlgorithmFactory
	{
		public static IPersistedAlgorithm GetPersistedAlgorithm(IExternalAlgorithm externalAlgorithm)
		{
			//Get the type by a big old switch
			return externalAlgorithm switch
			{
				External.AES_CBC x => new Persisted.AES_CBC(x),
				External.AES_CCM x => new Persisted.AES_CCM(x),
				External.AES_CMAC x => new Persisted.AES_CMAC(x),
				External.SHA2_224 x => new Persisted.SHA2_224(x),
				_ => null
			};
		}

	}
}
