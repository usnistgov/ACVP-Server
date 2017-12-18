using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, KdfResult>
    {
        private readonly IKdf _algo;

        public DeferredTestCaseResolver(IKdf algo)
        {
            _algo = algo;
        }

        public KdfResult CompleteDeferredCrypto(TestGroup group, TestCase serverTestCase, TestCase iutTestCase)
        {
            return _algo.DeriveKey(serverTestCase.KeyIn, iutTestCase.FixedData, group.KeyOutLength, serverTestCase.IV, iutTestCase.BreakLocation);
        }
    }
}
