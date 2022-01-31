using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class SafePrimesKeyVerResult
    {
        public SafePrimesKeyDisposition Disposition { get; set; }
        public FfcKeyPair KeyPair { get; set; }
        public bool TestPassed { get; set; }
    }
}
