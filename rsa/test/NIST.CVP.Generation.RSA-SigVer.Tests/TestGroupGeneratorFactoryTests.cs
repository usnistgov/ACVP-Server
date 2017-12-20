using Moq;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            var primeMock = new Mock<RandomProbablePrimeGenerator>();
            primeMock
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(3, 5));

            var smallPrimeMock = new Mock<AllProvablePrimesWithConditionsGenerator>();
            smallPrimeMock
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(3, 5));

            _subject = new TestGroupGeneratorFactory(primeMock.Object, smallPrimeMock.Object);
        }

        [Test]
        [TestCase(typeof(TestGroupGeneratorGeneratedDataTest))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators();
            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainOneGenerator()
        {
            var result = _subject.GetTestGroupGenerators();
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators();

            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigVer",
                IsSample = false,
                Capabilities = BuildFullSpecs(),
                PubExpMode = "random"
            };

            var groups = new List<ITestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators();

            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigVer",
                IsSample = false,
                Capabilities = BuildFullSpecs(),
                PubExpMode = "random"
            };

            List<ITestGroup> groups = new List<ITestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(63 * ParameterValidator.VALID_MODULI.Length, groups.Count);
        }

        private AlgSpecs[] BuildFullSpecs()
        {
            var hashPairs = new HashPair[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = i * 8
                };
            }

            var modCap = new CapSigType[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < modCap.Length; i++)
            {
                modCap[i] = new CapSigType
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashPairs = hashPairs
                };
            }

            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_VER_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_VER_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

            return algSpecs;
        }
    }
}
