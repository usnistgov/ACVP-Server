using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    public class FakeKdfFactory_BadZ : IKdfOneStepFactory
    {
        private readonly IKdfOneStepFactory _kdfFactory;

        public FakeKdfFactory_BadZ(IKdfOneStepFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IKdfOneStep GetInstance(KasKdfOneStepAuxFunction auxFunction)
        {
            var kdf = _kdfFactory.GetInstance(auxFunction);
            
            return new FakeKdf_BadZ(kdf);
        }
    }

    internal class FakeKdf_BadZ : IKdfOneStep
    {
        private readonly IKdfOneStep _kdf;

        public FakeKdf_BadZ(IKdfOneStep kdf)
        {
            _kdf = kdf;
        }

        public KdfResult DeriveKey(BitString z, int keyDataLength, BitString otherInfo, BitString salt)
        {
            // change the z prior to passing to the KDF
            z[0] += 2;

            return _kdf.DeriveKey(z, keyDataLength, otherInfo, salt);
        }
    }
}