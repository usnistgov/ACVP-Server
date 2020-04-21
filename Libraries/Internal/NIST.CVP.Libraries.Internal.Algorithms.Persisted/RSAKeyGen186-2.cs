using System.Collections.Generic;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class RSAKeyGen186_2 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("modLength")]
		public List<int> ModLengths { get; set; }

		[AlgorithmProperty("pubKeyValue")]
		public List<int> PublicKeyValues { get; set; }

		public RSAKeyGen186_2()
		{
			Name = "RSA";
			Mode = "keyGen";
			Revision = "186-2";
		}

		public RSAKeyGen186_2(External.RSAKeyGen186_2 external) : this()
		{
			//External doesn't use either of the properties...
		}
	}	
}
