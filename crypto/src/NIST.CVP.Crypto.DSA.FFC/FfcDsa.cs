using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcDsa : IDsaFfc
    {
        public ISha Sha { get; }

        private PQGeneratorFactory _pqGeneratorFactory = new PQGeneratorFactory();
        private GGeneratorFactory _gGeneratorFactory = new GGeneratorFactory();

        public FfcDsa(ISha sha)
        {
            Sha = sha;
        }

        public FfcDomainParametersGenerateResult GenerateDomainParameters(FfcDomainParametersGenerateRequest generateRequest)
        {
            // Generate p and q
            var pqGenerator = _pqGeneratorFactory.GetGenerator(generateRequest.PrimeGen);
            var pqResult = pqGenerator.Generate(generateRequest.PLength, generateRequest.QLength, generateRequest.SeedLength);
            if (!pqResult.Success)
            {
                return new FfcDomainParametersGenerateResult($"Failed to generate p and q with error: {pqResult.ErrorMessage}");
            }

            // Generate g
            var gGenerator = _gGeneratorFactory.GetGenerator(generateRequest.GeneratorGen);
            var gResult = gGenerator.Generate(pqResult.P, pqResult.Q);
            if (!gResult.Success)
            {
                return new FfcDomainParametersGenerateResult($"Failed to generate g with error: {gResult.ErrorMessage}");
            }

            var domainParameters = new FfcDomainParameters(pqResult.P, pqResult.Q, gResult.G);
            return new FfcDomainParametersGenerateResult(domainParameters, pqResult.Seed, pqResult.Counter);
        }

        public FfcKeyPairGenerateResult GenerateKeyPair(FfcDomainParameters domainParameters)
        {
            throw new NotImplementedException();
        }

        public FfcKeyPairValidateResult ValidateKeyPair(FfcDomainParameters domainParameters, FfcKeyPair keyPair)
        {
            throw new NotImplementedException();
        }
    }
}
