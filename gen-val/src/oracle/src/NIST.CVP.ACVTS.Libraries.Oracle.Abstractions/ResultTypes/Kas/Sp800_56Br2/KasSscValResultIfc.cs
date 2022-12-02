using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2
{
    public class KasSscValResultIfc
    {
        public bool TestPassed { get; set; }
        public KasSscTestCaseExpectation Disposition { get; set; }

        public KeyPair ServerKeyPair { get; set; }
        public BitString ServerC { get; set; }
        public BitString ServerZ { get; set; }

        public KeyPair IutKeyPair { get; set; }
        public BitString IutC { get; set; }
        public BitString IutZ { get; set; }

        public BitString Z { get; set; }

        public BitString IutHashZ { get; set; }
        public BitString HashZ { get; set; }

        public KasResult KasResult { get; set; }
    }
}
