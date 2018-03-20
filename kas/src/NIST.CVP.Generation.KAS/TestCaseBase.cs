using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestCaseBase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public TestCaseDispositionOption TestCaseDisposition { get; set; }

        public BitString DkmNonceServer { get; set; }
        public BitString EphemeralNonceServer { get; set; }

        public BitString DkmNonceIut { get; set; }
        public BitString EphemeralNonceIut { get; set; }
        
        
        public int IdIutLen { get; set; }
        public BitString IdIut { get; set; }

        public int OiLen { get; set; }
        public BitString OtherInfo { get; set; }
        
        public BitString NonceNoKc { get; set; }

        public BitString NonceAesCcm { get; set; }

        public BitString Z { get; set; }
        public BitString Dkm { get; set; }
        public BitString MacData { get; set; }

        public BitString HashZ { get; set; }
        public BitString Tag { get; set; }
        public string Result { get; set; }
    }
}
