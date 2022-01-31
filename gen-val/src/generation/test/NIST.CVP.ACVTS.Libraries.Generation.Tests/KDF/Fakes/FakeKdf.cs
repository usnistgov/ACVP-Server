using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.Fakes
{
    public class FakeKdf : IKdf
    {
        public KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv = null, int breakLocation = 0)
        {
            throw new NotImplementedException();
        }
    }
}
