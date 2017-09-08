using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class FfcSharedInformation
    {
        /// <summary>
        /// The domain parameters involved in keypair generation
        /// </summary>
        public FfcDomainParameters DomainParameters { get; }
        /// <summary>
        /// The identifier of this party.
        /// </summary>
        public BitString PartyId { get; }
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

        public FfcSharedInformation(FfcDomainParameters domainParameters, BitString partyId, BigInteger staticPublicKey, BigInteger ephemeralPublicKey, BitString ephemeralNonce, BitString dkmNonce, BitString noKeyConfirmationNonce)
        {
            DomainParameters = domainParameters;
            PartyId = partyId;
            StaticPublicKey = staticPublicKey;
            EphemeralPublicKey = ephemeralPublicKey;
            EphemeralNonce = ephemeralNonce;
            DkmNonce = dkmNonce;
            NoKeyConfirmationNonce = noKeyConfirmationNonce;
        }
    }
}