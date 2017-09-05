using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class FfcSharedInformation
    {
        public FfcDomainParameters DomainParameters { get; }
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
        /// <summary>
        /// A nonce utilized within MACData for no key confirmation KAS
        /// </summary>
        public BitString NoKeyConfirmationNonce { get; }

        public FfcSharedInformation(FfcDomainParameters domainParameters, BigInteger staticPublicKey, BigInteger ephemeralPublicKey, BitString ephemeralNonce, BitString dkmNonce, BitString noKeyConfirmationNonce)
        {
            DomainParameters = domainParameters;
            StaticPublicKey = staticPublicKey;
            EphemeralPublicKey = ephemeralPublicKey;
            EphemeralNonce = ephemeralNonce;
            DkmNonce = dkmNonce;
            NoKeyConfirmationNonce = noKeyConfirmationNonce;
        }
    }
}