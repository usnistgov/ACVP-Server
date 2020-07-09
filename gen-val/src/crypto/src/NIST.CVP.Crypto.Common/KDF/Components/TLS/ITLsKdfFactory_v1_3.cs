using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;

namespace NIST.CVP.Crypto.Common.KDF.Components.TLS
{
	public interface ITLsKdfFactory_v1_3
	{
		ITlsKdf_v1_3 GetInstance(HashFunctions hashFunction);
	}
}