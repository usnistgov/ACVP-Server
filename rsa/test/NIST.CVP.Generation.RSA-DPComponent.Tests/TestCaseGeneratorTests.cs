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

namespace NIST.CVP.Generation.RSA_DPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCDEFABCDEF"));        // Must be between 32-64 bits

            var subject = new TestCaseGenerator(rand.Object, GetKeyBuilderMock().Object, GetKeyComposerFactoryMock().Object, GetRsaMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IRsa> GetRsaMock()
        {
            return new Mock<IRsa>();
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
                Modulo = 2048
            };
        }
    }
}
