using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF.Tests.Fakes
{
    public class FakeKdf : IKdf
    {
        public KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv = null, int breakLocation = 0)
        {
            throw new NotImplementedException();
        }
    }
}
