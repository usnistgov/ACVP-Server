using System.Linq;
using System.Numerics;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.TestCaseExpectations;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignature()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One * 3, testCase.KeyPair.PrivateD);
        }

        [Test]
        public void GenerateShouldGenerateProperAmountOfEachFailureReason()
        {
            var group = GetTestGroup();

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(group, false);

                Assert.IsTrue(result.Success);
                group.Tests.Add(result.TestCase);
            }

            var failCases = 0;
            var passCases = 0;
            foreach (var testCase in group.Tests.Select(s => s))
            {
                if (testCase.TestPassed != null && testCase.TestPassed.Value)
                {
                    passCases++;
                }
                else
                {
                    failCases++;
                }
            }

            Assert.AreEqual(12, failCases);
            Assert.AreEqual(3, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));
            return randMock;
        }

        private Mock<IDsaEcc> GetDsaMock()
        {
            var eccMock = new Mock<IDsaEcc>();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            eccMock
                .Setup(s => s.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new EccSignatureResult(new EccSignature(1, 2)));
            return eccMock;
        }

        private Mock<IDsaEccFactory> GetEccFactoryMock()
        {
            var mock = new Mock<IDsaEccFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(GetDsaMock().Object);
            return mock;
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            var mock = new Mock<IEccCurveFactory>();
            mock
                .Setup(s => s.GetCurve(It.IsAny<Curve>()))
                .Returns(new PrimeCurve(Curve.P192, 0, 0, new EccPoint(0, 0), 0));
            return mock;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.P192,
                TestCaseExpectationProvider = new TestCaseExpectationProvider(),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256)
            };
        }
    }
}
