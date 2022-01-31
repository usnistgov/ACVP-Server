using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2
{
    public class KasSscAftResultIfc
    {
        public KeyPair ServerKeyPair { get; set; }
        public BitString ServerC { get; set; }
        public BitString ServerZ { get; set; }

        public KeyPair IutKeyPair { get; set; }
    }
}
