using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, EccVerificationResult>
    {
        private readonly IDsaEccFactory _eccDsaFactory;
        private readonly IEccCurveFactory _curveFactory;

        public DeferredTestCaseResolver(IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

        public EccVerificationResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = (TestGroup) iutTestCase.Parent;
            if (iutTestGroup.KeyPair == null)
            {
                return new EccVerificationResult("Could not find Q");
            }

            var eccDsa = _eccDsaFactory.GetInstance(serverTestGroup.HashAlg);
            var curve = _curveFactory.GetCurve(serverTestGroup.Curve);
            var domainParams = new EccDomainParameters(curve);

            var verifyResult = eccDsa.Verify(domainParams, iutTestGroup.KeyPair, serverTestCase.Message, iutTestCase.Signature);

            return verifyResult;
        }
    }
}
