using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Scheme
{
    public class OtherPartySharedInformation<TDomainParameters, TKeyPair> 
        : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// The domain parameters involved in keypair generation
        /// </summary>
        public TDomainParameters DomainParameters { get; }
        /// <summary>
        /// The identifier of this party.
        /// </summary>
        public BitString PartyId { get; }
        /// <summary>
        /// This Party's static public key
        /// </summary>
        public TKeyPair StaticPublicKey { get; }
        /// <summary>
        /// This Party's ephemeral public key
        /// </summary>
        public TKeyPair EphemeralPublicKey { get; }
        /// <summary>
        /// The DKM Nonce
        /// </summary>
        public BitString DkmNonce { get; }
        /// <summary>
        /// This party's ephemeral nonce
        /// </summary>
        public BitString EphemeralNonce { get; }
        /// <summary>
        /// A nonce utilized within MACData for no key confirmation KAS
        /// </summary>
        public BitString NoKeyConfirmationNonce { get; }

        public OtherPartySharedInformation(TDomainParameters domainParameters, BitString partyId, TKeyPair staticPublicKey, TKeyPair ephemeralPublicKey, BitString dkmNonce, BitString ephemeralNonce, BitString noKeyConfirmationNonce)
        {
            DomainParameters = domainParameters;
            PartyId = partyId;
            StaticPublicKey = staticPublicKey;
            EphemeralPublicKey = ephemeralPublicKey;
            DkmNonce = dkmNonce;
            EphemeralNonce = ephemeralNonce;
            NoKeyConfirmationNonce = noKeyConfirmationNonce;
        }
    }
}