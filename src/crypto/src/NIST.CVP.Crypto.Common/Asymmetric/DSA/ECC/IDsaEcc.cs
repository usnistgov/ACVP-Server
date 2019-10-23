﻿namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IDsaEcc : IDsa<
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
    { }
}