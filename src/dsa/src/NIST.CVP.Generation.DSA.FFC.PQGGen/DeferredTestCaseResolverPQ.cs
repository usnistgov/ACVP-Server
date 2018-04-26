using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class DeferredTestCaseResolverPQ : IDeferredTestCaseResolver<TestGroup, TestCase, PQValidateResult>
    {
        private readonly IPQGeneratorValidatorFactory _pqGenFactory;
        private readonly IShaFactory _shaFactory;

        public DeferredTestCaseResolverPQ(IPQGeneratorValidatorFactory pqGenFactory, IShaFactory shaFactory)
        {
            _pqGenFactory = pqGenFactory;
            _shaFactory = shaFactory;
        }
        
        public PQValidateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var sha = _shaFactory.GetShaInstance(serverTestGroup.HashAlg);
            var pqGen = _pqGenFactory.GetGeneratorValidator(serverTestGroup.PQGenMode, sha, EntropyProviderTypes.Random);

            return pqGen.Validate(iutTestCase.P, iutTestCase.Q, serverTestCase.Seed, iutTestCase.Counter);
        }
    }
}
