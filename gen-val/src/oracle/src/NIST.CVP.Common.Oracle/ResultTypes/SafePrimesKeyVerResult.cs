using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class SafePrimesKeyVerResult
    {
        public SafePrimesKeyDisposition Disposition { get; set; }
        public FfcKeyPair KeyPair { get; set; }
        public bool TestPassed { get; set; }
    }
}