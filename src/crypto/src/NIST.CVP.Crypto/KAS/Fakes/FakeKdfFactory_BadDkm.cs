using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    public class FakeKdfFactory_BadDkm : IKdfOneStepFactory
    {
        private readonly IKdfOneStepFactory _kdfFactory;

        public FakeKdfFactory_BadDkm(IKdfOneStepFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IKdfOneStep GetInstance(KasKdfOneStepAuxFunction auxFunction)
        {
            var kdf = _kdfFactory.GetInstance(auxFunction);
            
            return new FakeKdf_BadDkm(kdf);
        }
    }

    internal class FakeKdf_BadDkm : IKdfOneStep
    {
        private readonly IKdfOneStep _kdf;

        public FakeKdf_BadDkm(IKdfOneStep kdf)
        {
            _kdf = kdf;
        }

        public KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo, BitString salt)
        {
            var dkmResult = _kdf.DeriveKey(z, keyDataLength, otherInfo, salt);

            // Change the DKM prior to returning
            dkmResult.DerivedKey[0] += 2;

            return dkmResult;
        }
    }
}