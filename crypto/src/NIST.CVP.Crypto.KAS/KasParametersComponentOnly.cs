using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public class KasParametersComponentOnly : IKasParameters
    {
        public KasParametersComponentOnly(KeyAgreementRole keyAgreementRole, FfcScheme scheme, FfcParameterSet parameterSet, KasAssurance assurances, IDsaFfc dsa, BitString partyId)
        {
            KeyAgreementRole = keyAgreementRole;
            Scheme = scheme;
            ParameterSet = parameterSet;
            Assurances = assurances;
            Dsa = dsa;
            PartyId = partyId;
        }

        /// <inheritdoc />
        public KeyAgreementRole KeyAgreementRole { get; }
        /// <inheritdoc />
        public FfcScheme Scheme { get; }
        /// <inheritdoc />
        public FfcParameterSet ParameterSet { get; }
        /// <inheritdoc />
        public KasAssurance Assurances { get; }
        /// <inheritdoc />
        public IDsaFfc Dsa { get; }
        /// <inheritdoc />
        public BitString PartyId { get; }
    }
}