﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
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
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetFactoryMock(dsaMock).Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignature()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetFactoryMock(dsaMock).Object);

            var result = subject.Generate(GetTestGroup(), true);

            dsaMock.Verify(v => v.GenerateKeyPair(It.IsAny<FfcDomainParameters>()), Times.Once, "Call KeyGen Generate once");
            dsaMock.Verify(v => v.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()), Times.Once, "Call Sign once");

            Assert.IsTrue(result.Success);
            var testCase = result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.Key.PrivateKeyX);
        }

        [Test]
        public void GenerateShouldGenerateProperAmountOfEachFailureReason()
        {
            var group = GetTestGroup();

            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetFactoryMock(dsaMock).Object);

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

            Assert.AreEqual(8, failCases);
            Assert.AreEqual(7, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            return randMock;
        }

        private Mock<IDsaFfc> GetDsaMock()
        {
            return new Mock<IDsaFfc>();
        }

        private Mock<IDsaFfcFactory> GetFactoryMock(IMock<IDsaFfc> dsaMock)
        {
            var dsaFactoryMock = new Mock<IDsaFfcFactory>();
            dsaFactoryMock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(dsaMock.Object);

            return dsaFactoryMock;
        }
        
        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
                DomainParams = new FfcDomainParameters(1, 2, 3),
                TestCaseExpectationProvider = new TestCaseExpectationProvider(),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256)
            };
        }
    }
}