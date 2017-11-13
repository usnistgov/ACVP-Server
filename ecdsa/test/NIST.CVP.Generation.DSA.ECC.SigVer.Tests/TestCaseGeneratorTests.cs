using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.TestCaseExpectations;
using NIST.CVP.Math;
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
            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var eccMock = GetDsaMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            eccMock
                .Setup(s => s.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>()))
                .Returns(new EccSignatureResult(new EccSignature(1, 2)));

            var subject = new TestCaseGenerator(randMock.Object, eccMock.Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignature()
        {
            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var eccMock = GetDsaMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            eccMock
                .Setup(s => s.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>()))
                .Returns(new EccSignatureResult(new EccSignature(1, 2)));

            var subject = new TestCaseGenerator(randMock.Object, eccMock.Object);

            var result = subject.Generate(GetTestGroup(), true);

            eccMock.Verify(v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), Times.AtLeastOnce, "Call KeyGen Generate at least once");
            eccMock.Verify(v => v.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>()), Times.Once, "Call Sign once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One * 3, testCase.KeyPair.PrivateD);
        }

        [Test]
        public void GenerateShouldGenerateProperAmountOfEachFailureReason()
        {
            var group = GetTestGroup();

            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var eccMock = GetDsaMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            eccMock
                .Setup(s => s.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>()))
                .Returns(new EccSignatureResult(new EccSignature(1, 2)));

            var subject = new TestCaseGenerator(randMock.Object, eccMock.Object);

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

            Assert.AreEqual(12, failCases);
            Assert.AreEqual(3, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaEcc> GetDsaMock()
        {
            return new Mock<IDsaEcc>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                DomainParameters = new EccDomainParameters(new PrimeCurve(Curve.P192, 0, 0, new EccPoint(0, 0), 0)),
                TestCaseExpectationProvider = new TestCaseExpectationProvider(),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256)
            };
        }
    }
}
