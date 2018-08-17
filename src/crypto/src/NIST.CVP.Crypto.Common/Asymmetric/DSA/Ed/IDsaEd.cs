﻿using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IDsaEd : IDsa<
        EdDomainParametersGenerateRequest,
        EdDomainParametersGenerateResult,
        EdDomainParametersValidateRequest,
        EdDomainParametersValidateResult,
        EdDomainParameters,
        EdKeyPairGenerateResult,
        EdKeyPair,
        EdKeyPairValidateResult,
        EdSignature,
        EdSignatureResult,
        EdVerificationResult
        >
    {
        void AddEntropy(BigInteger entropy);
        EdSignatureResult Sign(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, BitString context, bool preHash = false);
        EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, BitString context, bool preHash = false);
    }
}