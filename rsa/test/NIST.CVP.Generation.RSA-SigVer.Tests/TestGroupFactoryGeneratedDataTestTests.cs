using Moq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

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
                    .WithHashAlgs(new [] {"SHA-1", "SHA-224"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pkcs1v15"})
                    .WithModuli(new [] {2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA-256", "SHA-512/224", "SHA-512/256"})
                    .Build()
            },
            new object[]
            {
                63,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss", "pkcs1v15", "ansx9.31"})
                    .WithModuli(new [] {1024, 2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA-512/224", "SHA-512/256"})
                    .Build()
            },
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate3TestGroupsForEachCombinationOfModeModuloAndHashAlg(int expectedGroups, Parameters parameters)
        {
            var primeMock = new Mock<RandomProbablePrimeGenerator>();
            primeMock
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(3, 5));

            var subject = new TestGroupGeneratorGeneratedDataTest(primeMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups * 3, result.Count());
        }

        [Test]
        public void AllTestGroupsCreatedShouldHaveProvidedFixedEValue()
        {
            var eValue = "01234567";
            var parameters = new ParameterValidatorTests.ParameterBuilder()
                .WithSigVerModes(new[] { "ansx9.31", "pss" })
                .WithModuli(new[] { 1024, 2048, 3072 })
                .WithHashAlgs(new[] { "SHA-256", "SHA-512" })
                .WithEValue(eValue)
                .Build();

            var primeMock = new Mock<RandomProbablePrimeGenerator>();
            primeMock
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(3, 5));

            var subject = new TestGroupGeneratorGeneratedDataTest(primeMock.Object);
            var result = subject.BuildTestGroups(parameters);

            foreach(var group in result.Select(s => (TestGroup)s).ToList())
            {
                Assert.AreEqual(eValue, new BitString(group.Key.PubKey.E).ToHex());
            }
        }
    }
}
