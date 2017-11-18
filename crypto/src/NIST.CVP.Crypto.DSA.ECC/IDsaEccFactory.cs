using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public interface IDsaEccFactory
        : IDsaFactory<
            IDsaEcc,
            EccDomainParametersGenerateRequest,
            EccDomainParametersGenerateResult,
            EccDomainParametersValidateRequest,
            EccDomainParametersValidateResult,
            EccDomainParameters,
            EccKeyPairGenerateResult,
            EccKeyPair,
            EccKeyPairValidateResult,
            EccSignature,
            EccSignatureResult,
            EccVerificationResult
        >
    {
    }
}
