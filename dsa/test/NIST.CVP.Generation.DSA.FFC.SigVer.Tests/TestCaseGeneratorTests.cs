using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.Tests
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

            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(randMock.Object, dsaMock.Object);
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

            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(randMock.Object, dsaMock.Object);

            var result = subject.Generate(GetTestGroup(), true);

            dsaMock.Verify(v => v.GenerateKeyPair(It.IsAny<FfcDomainParameters>()), Times.Once, "Call KeyGen Generate once");
            dsaMock.Verify(v => v.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>()), Times.Once, "Call Sign once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.Key.PrivateKeyX);
        }

        [Test]
        public void GenerateShouldGenerateOneOfEachFailureReason()
        {
            var group = GetTestGroup();

            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(randMock.Object, dsaMock.Object);

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
            Assert.AreEqual(7, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaFfc> GetDsaMock()
        {
            return new Mock<IDsaFfc>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
                DomainParams = new FfcDomainParameters(1, 2, 3),
                FailureHandler = new FailureHandler(),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256)
            };
        }
    }
}
