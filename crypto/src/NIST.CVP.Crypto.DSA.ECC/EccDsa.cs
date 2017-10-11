using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccDsa : IDsaEcc
    {
        public ISha Sha { get; }

        public EccDsa(ISha sha)
        {
            Sha = sha;
        }

        public EccDomainParametersGenerateResult GenerateDomainParameters(EccDomainParametersGenerateRequest generateRequest)
        {
            throw new NotImplementedException();
        }

        public EccDomainParametersValidateResult ValidateDomainParameters(EccDomainParametersValidateRequest domainParameters)
        {
            throw new NotImplementedException();
        }

        public EccKeyPairGenerateResult GenerateKeyPair(EccDomainParameters domainParameters)
        {
            throw new NotImplementedException();
        }

        public EccKeyPairValidateResult ValidateKeyPair(EccDomainParameters domainParameters, EccKeyPair keyPair)
        {
            throw new NotImplementedException();
        }

        public EccSignatureResult Sign(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message)
        {
            throw new NotImplementedException();
        }

        public EccVerificationResult Verify(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message, EccSignature signature)
        {
            throw new NotImplementedException();
        }
    }
}
