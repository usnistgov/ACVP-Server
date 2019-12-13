using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.FixedInfo
{
    /// <summary>
    /// Class is used to represent contextual information as it pertains to KAS - KDFs, KeyConfirmation, etc.
    /// </summary>
    public class FixedInfoParameter
    {
        /// <summary>
        /// The pattern and order to write the fixed info
        /// </summary>
        public string FixedInfoPattern { get; set; }

        /// <summary>
        /// The length of the key that is to be derived or transported.
        /// </summary>
        public int L { get; set; }

        /// <summary>
        /// The Salt value used for a MAC operation for KDF.
        /// </summary>
        public BitString Salt { get; set; }

        /// <summary>
        /// The initialization vector used in some MAC operations.
        /// </summary>
        public BitString Iv { get; set; }

        /// <summary>
        /// The encoding type to use on the fixed info data.
        /// </summary>
        public FixedInfoEncoding Encoding { get; set; }

        /// <summary>
        /// Party U's fixed info contribution.
        /// </summary>
        public PartyFixedInfo FixedInfoPartyU { get; private set; }

        /// <summary>
        /// Party V's fixed info contribution.
        /// </summary>
        public PartyFixedInfo FixedInfoPartyV { get; private set; }

        /// <summary>
        /// The algorithm ID indicator.
        /// </summary>
        public BitString AlgorithmId { get; set; }

        /// <summary>
        /// The Label for the transaction.
        /// </summary>
        public BitString Label { get; set; }

        /// <summary>
        /// The Context for the transaction.
        /// </summary>
        public BitString Context { get; set; }

        /// <summary>
        /// Sets the fixed info from the two contributing parties.
        /// </summary>
        /// <param name="partyU">The party U (initiator) fixed info.</param>
        /// <param name="partyV">The party V (responder) fixed info.</param>
        public void SetFixedInfo(PartyFixedInfo partyU, PartyFixedInfo partyV)
        {
            FixedInfoPartyU = partyU;
            FixedInfoPartyV = partyV;
        }
    }
}