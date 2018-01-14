﻿namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public interface IDsaFfc : 
        IDsa<
            FfcDomainParametersGenerateRequest, 
            FfcDomainParametersGenerateResult, 
            FfcDomainParametersValidateRequest,
            FfcDomainParametersValidateResult,
            FfcDomainParameters, 
            FfcKeyPairGenerateResult,
            FfcKeyPair, 
            FfcKeyPairValidateResult,
            FfcSignature,
            FfcSignatureResult,
            FfcVerificationResult
        >
    {
    }
}