using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, FfcVerificationResult>
    {
        private readonly IDsaFfcFactory _dsaFactory;

        public DeferredTestCaseResolver(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public FfcVerificationResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;

            if (iutTestGroup.DomainParams == null)
            {
                return new FfcVerificationResult("Could not find p, q or g");
            }

            var ffcDsa = _dsaFactory.GetInstance(serverTestGroup.HashAlg);
            var verifyResult = ffcDsa.Verify(iutTestGroup.DomainParams, iutTestCase.Key, serverTestCase.Message, iutTestCase.Signature);
            return verifyResult;
        }
    }
}
