using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class DeferredTestCaseResolverG : IDeferredTestCaseResolver<TestGroup, TestCase, GValidateResult>
    {
        private readonly IGGeneratorValidatorFactory _gGenFactory;
        private readonly IShaFactory _shaFactory;

        public DeferredTestCaseResolverG(IGGeneratorValidatorFactory gGenFactory, IShaFactory shaFactory)
        {
            _gGenFactory = gGenFactory;
            _shaFactory = shaFactory;
        }
        
        public GValidateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var sha = _shaFactory.GetShaInstance(serverTestGroup.HashAlg);
            var gGen = _gGenFactory.GetGeneratorValidator(serverTestGroup.GGenMode, sha);

            return gGen.Validate(serverTestCase.P, serverTestCase.Q, iutTestCase.G, serverTestCase.Seed, serverTestCase.Index);
        }
    }
}
