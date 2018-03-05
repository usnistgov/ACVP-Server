using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, EccKeyPairGenerateResult>
    {
        private readonly IDsaEccFactory _dsaEccFactory;
        private readonly IEccCurveFactory _curveFactory;

        public DeferredTestCaseResolver(IDsaEccFactory dsaEccFactory, IEccCurveFactory curveFactory)
        {
            _dsaEccFactory = dsaEccFactory;
            _curveFactory = curveFactory;
        }

        public EccKeyPairGenerateResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d224);
            var eccDsa = _dsaEccFactory.GetInstance(hashFunction, EntropyProviderTypes.Testable);
            eccDsa.AddEntropy(iutTestCase.KeyPair.PrivateD);

            var curve = _curveFactory.GetCurve(serverTestGroup.Curve);
            var domainParameters = new EccDomainParameters(curve);
            var generateResult = eccDsa.GenerateKeyPair(domainParameters);

            return generateResult;
        }
    }
}
