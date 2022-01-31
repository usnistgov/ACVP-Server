using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo
{
    /// <summary>
    /// Represents the information contributed to other info for a single party
    /// </summary>
    public class PartyOtherInfo
    {
        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="partyId">The party's ID</param>
        /// <param name="dkmNonce">The party's DKM Nonce</param>
        public PartyOtherInfo(BitString partyId, BitString dkmNonce)
        {
            PartyId = partyId;
            DkmNonce = dkmNonce;
        }

        /// <summary>
        /// The party's ID
        /// </summary>
        public BitString PartyId { get; }
        /// <summary>
        /// The party's DKM nonce (used for static schemes)
        /// </summary>
        public BitString DkmNonce { get; }
    }
}
