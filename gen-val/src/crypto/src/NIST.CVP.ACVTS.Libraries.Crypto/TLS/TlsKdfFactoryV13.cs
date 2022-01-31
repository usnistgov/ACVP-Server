using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TLS
{
    public class TlsKdfFactoryV13 : ITLsKdfFactory_v1_3
    {
        private readonly IHkdfFactory _hkdfFactory;
        private readonly IShaFactory _shaFactory;

        public TlsKdfFactoryV13(IHkdfFactory hkdfFactory, IShaFactory shaFacory)
        {
            _hkdfFactory = hkdfFactory;
            _shaFactory = shaFacory;
        }

        public ITlsKdf_v1_3 GetInstance(HashFunctions hashFunction)
        {
            var hf = ShaAttributes.GetHashFunctionFromEnum(hashFunction);

            return new TlsKdfv13(_hkdfFactory.GetKdf(hf), _shaFactory.GetShaInstance(hf), hf.OutputLen);
        }
    }
}
