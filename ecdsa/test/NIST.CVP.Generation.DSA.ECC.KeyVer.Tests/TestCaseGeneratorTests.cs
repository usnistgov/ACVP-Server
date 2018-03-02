using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;
using NIST.CVP.Generation.DSA.ECC.KeyVer.TestCaseExpectations;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBigInteger(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var subject = new TestCaseGenerator(rand.Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("AB"));

            var subject = new TestCaseGenerator(rand.Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;

            Assert.AreEqual(BigInteger.One * 3, testCase.KeyPair.PrivateD);
        }

        [Test]
        public void GenerateShouldGenerateOneOfEachFailureReason()
        {
            var group = GetTestGroup();

            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var subject = new TestCaseGenerator(rand.Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(group, false);

                Assert.IsTrue(result.Success);
                group.Tests.Add(result.TestCase);
            }

            var failCases = 0;
            var passCases = 0;
            foreach (var testCase in group.Tests.Select(s => (TestCase)s))
            {
                if (testCase.FailureTest)
                {
                    failCases++;
                }
                else
                {
                    passCases++;
                }
            }

            Assert.AreEqual(8, failCases);
            Assert.AreEqual(4, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaEcc> GetEccMock()
        {
            var eccMock = new Mock<IDsaEcc>();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));
            return eccMock;
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            var mock = new Mock<IEccCurveFactory>();
            mock
                .Setup(s => s.GetCurve(It.IsAny<Curve>()))
                .Returns(GetCurveMock().Object);
            return mock;
        }

        private Mock<IDsaEccFactory> GetEccFactoryMock()
        {
            var mock = new Mock<IDsaEccFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(GetEccMock().Object);
            return mock;
        }

        private Mock<IEccCurve> GetCurveMock()
        {
            var mock = new Mock<IEccCurve>();
            mock
                .Setup(s => s.PointExistsInField(It.IsAny<EccPoint>()))
                .Returns(false);
            return mock;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.B163,
                TestCaseExpectationProvider = new TestCaseExpectationProvider()
            };
        }
    }
}
