using Moq;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupFactoryGeneratedDataTestTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss"})
                    .WithModuli(new [] {2048})
                    .WithHashAlgs(new [] {"SHA-1"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss", "ansx9.31"})
                    .WithModuli(new [] {2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pkcs1v1.5"})
                    .WithModuli(new [] {2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-256", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            },
            new object[]
            {
                63,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss", "pkcs1v1.5", "ansx9.31"})
                    .WithModuli(new [] {1024, 2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            },
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate3TestGroupsForEachCombinationOfModeModuloAndHashAlg(int expectedGroups, Parameters parameters)
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCDEFABCDEF"));            // Needs to be between 32/64 bits

            var keyBuilderMock = new Mock<IKeyBuilder>();
            keyBuilderMock
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair(), new AuxiliaryResult()));
            keyBuilderMock.SetReturnsDefault(keyBuilderMock.Object);

            var keyComposerFactoryMock = new Mock<IKeyComposerFactory>();

            var shaFactoryMock = new Mock<IShaFactory>();

            var subject = new TestGroupGeneratorGeneratedDataTest(randMock.Object, keyBuilderMock.Object, keyComposerFactoryMock.Object, shaFactoryMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups * 3, result.Count());
        }
    }
}
