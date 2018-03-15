using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, FfcKeyPairValidateResult>
    {
        private readonly IDsaFfcFactory _dsaFactory;

        public DeferredTestCaseResolver(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public FfcKeyPairValidateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;
            if (iutTestGroup.DomainParams.P == 0 || iutTestGroup.DomainParams.Q == 0 || iutTestGroup.DomainParams.G == 0)
            {
                return new FfcKeyPairValidateResult("Could not find p, q, or g");
            }

            var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
            var ffcDsa = _dsaFactory.GetInstance(hashFunction);
            var validateResult = ffcDsa.ValidateKeyPair(iutTestGroup.DomainParams, iutTestCase.Key);
            return validateResult;
        }
    }
}
