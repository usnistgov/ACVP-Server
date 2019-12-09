using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public abstract class KasValResultBase
    {
        public bool TestPassed { get; set; }

        public KasValTestDisposition KasValTestDisposition { get; set; }

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
    }
}
