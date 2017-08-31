using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class FfcSharedInformation
    {
        /// <summary>
        /// This Party's static public key
        /// </summary>
        public BigInteger StaticPublicKey { get; }
        /// <summary>
        /// This Party's ephemeral public key
        /// </summary>
        public BigInteger EphemeralPublicKey { get; }
        /// <summary>
        /// This party's ephemeral nonce
        /// </summary>
        public BitString EphemeralNonce { get; }
        /// <summary>
        /// The DKM Nonce
        /// </summary>
        public BitString DkmNonce { get; }

        public FfcSharedInformation(BigInteger staticPublicKey, BigInteger ephemeralPublicKey, BitString ephemeralNonce, BitString dkmNonce)
        {
            StaticPublicKey = staticPublicKey;
            EphemeralPublicKey = ephemeralPublicKey;
            EphemeralNonce = ephemeralNonce;
            DkmNonce = dkmNonce;
        }
    }
}