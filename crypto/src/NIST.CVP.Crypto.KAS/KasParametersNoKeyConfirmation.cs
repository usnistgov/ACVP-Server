using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS
{
    public class KasParametersNoKeyConfirmation : IKasParameters
    {
        public KasParametersNoKeyConfirmation(KeyAgreementRole keyAgreementRole, FfcScheme scheme, FfcParameterSet parameterSet, KasAssurance assurances, IDsaFfc dsa, BitString partyId, KdfParameters kdfParameters, MacParameters macParameters)
        {
            KeyAgreementRole = keyAgreementRole;
            Scheme = scheme;
            ParameterSet = parameterSet;
            Assurances = assurances;
            Dsa = dsa;
            PartyId = partyId;
            KdfParameters = kdfParameters;
            MacParameters = macParameters;
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
        /// <summary>
        /// The parameters used in the key derivation function
        /// </summary>
        public KdfParameters KdfParameters { get; }
        /// <summary>
        /// The parameters used in the MAC function.
        /// </summary>
        public MacParameters MacParameters { get; }
    }
}
