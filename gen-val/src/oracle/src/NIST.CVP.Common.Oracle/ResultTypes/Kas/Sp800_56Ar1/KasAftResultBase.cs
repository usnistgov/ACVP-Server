using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1
{
    public abstract class KasAftResultBase
    {
        public bool Deferred { get; set; } = true;

        public BitString DkmNonceServer { get; set; }

        public BitString EphemeralNonceServer { get; set; }


        public BitString NonceNoKc { get; set; }

        public BitString NonceAesCcm { get; set; }


        #region Sample Only Properties
        /*
            Note properties within this region are only ever populated
            When the test is being requested as a sample.

            This "frontloads" all the crypto of the request onto the 
            generation operation. In a non-sample scenario, only the server
            portion is created on the generation side.
        */

        public BitString DkmNonceIut { get; set; }

        public BitString EphemeralNonceIut { get; set; }


        public BitString IdIut { get; set; }


        public int OiLen { get; set; }

        public BitString OtherInfo { get; set; }


        public BitString Z { get; set; }

        public BitString Dkm { get; set; }

        public BitString MacData { get; set; }

        public BitString HashZ { get; set; }

        public BitString Tag { get; set; }
        #endregion Sample Only Properties
    }
}
