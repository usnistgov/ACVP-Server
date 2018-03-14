using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using Moq;
using System.Linq;

namespace NIST.CVP.Generation.RSA_DPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetKeyBuilderMock().Object, GetKeyComposerFactoryMock().Object, GetRsaMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnExactlyTenFailureCases()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetKeyBuilderMock().Object, GetKeyComposerFactoryMock().Object, GetRsaMock().Object);
            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsTrue(result.Success, result.ErrorMessage);

            var testCase = (TestCase) result.TestCase;
            Assert.AreEqual(GetTestGroup().TotalFailingCases, testCase.ResultsArray.Count(ra => ra.FailureTest));
            Assert.AreEqual(GetTestGroup().TotalTestCases, testCase.ResultsArray.Count);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var rand = new Mock<IRandom800_90>();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCDEFABCDEF"));        // Must be between 32-64 bits

            rand
                .Setup(s => s.GetRandomBigInteger(It.IsAny<BigInteger>(), It.IsAny<BigInteger>()))
                .Returns(1);
            
            return rand;
        }

        private Mock<IRsa> GetRsaMock()
        {
            var mock = new Mock<IRsa>();
            mock
                .Setup(s => s.Decrypt(It.IsAny<BigInteger>(), It.IsAny<PrivateKeyBase>(), It.IsAny<PublicKey>()))
                .Returns(new DecryptionResult(123));

            return mock;
        }

        private Mock<IKeyBuilder> GetKeyBuilderMock()
        {
            var mock = new Mock<IKeyBuilder>();
            mock
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair { PubKey = new PublicKey{N = 10}}, new AuxiliaryResult()));
            
            mock.SetReturnsDefault(mock.Object);
            
            return mock;
        }

        private Mock<IKeyComposerFactory> GetKeyComposerFactoryMock()
        {
            return new Mock<IKeyComposerFactory>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Modulo = 2048,
                TotalFailingCases = 10,
                TotalTestCases = 30
            };
        }
    }
}
