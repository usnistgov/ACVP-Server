using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public interface IKasParameters
    {
        /// <summary>
        /// This party's key agreement role
        /// </summary>
        KeyAgreementRole KeyAgreementRole { get; }
        /// <summary>
        /// The KAS scheme used
        /// </summary>
        FfcScheme Scheme { get; }
        /// <summary>
        /// The parameter set for the KAS
        /// </summary>
        FfcParameterSet ParameterSet { get; }
        /// <summary>
        /// The assurances associated with this KAS
        /// </summary>
        KasAssurance Assurances { get; }
        /// <summary>
        /// The DSA instance
        /// </summary>
        IDsaFfc Dsa { get; }
        /// <summary>
        /// The ID associated with this party
        /// </summary>
        BitString PartyId { get; }
    }
}