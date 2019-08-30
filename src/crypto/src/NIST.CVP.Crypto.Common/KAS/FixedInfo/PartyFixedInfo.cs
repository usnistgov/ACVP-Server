using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.FixedInfo
{
    /// <summary>
    /// Represents the information contributed to fixed info for a single party
    /// </summary>
    public class PartyFixedInfo
    {
            /// <summary>
            /// Constructs an instance
            /// </summary>
            /// <param name="partyId">The party's ID.</param>
            /// <param name="ephemeralData">The party's ephemeral data used for FixedInfo construction.</param>
            public PartyFixedInfo(BitString partyId, BitString ephemeralData)
            {
                PartyId = partyId;
                EphemeralData = ephemeralData;
            }

            /// <summary>
            /// The party's ID.
            /// </summary>
            public BitString PartyId { get; }
            /// <summary>
            /// The party's ephemeral data used for FixedInfo construction.
            /// </summary>
            public BitString EphemeralData { get; }
    }
}