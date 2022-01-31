using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Fakes
{
    public class FakeKdfFactory_BadDkm : IKdfOneStepFactory
    {
        private readonly IKdfOneStepFactory _kdfFactory;

        public FakeKdfFactory_BadDkm(IKdfOneStepFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IKdfOneStep GetInstance(KdaOneStepAuxFunction auxFunction, bool useCounter)
        {
            var kdf = _kdfFactory.GetInstance(auxFunction, useCounter);

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
