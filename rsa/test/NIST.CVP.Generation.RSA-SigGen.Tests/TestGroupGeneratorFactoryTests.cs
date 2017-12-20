using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new TestGroupGeneratorFactory();
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
                Mode = "SigGen",
                IsSample = false,
                Capabilities = BuildFullSpecs(),
            };

            var groups = new List<ITestGroup>();

            foreach(var genny in result)
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
                Mode = "SigGen",
                IsSample = false,
                Capabilities = BuildFullSpecs(),
            };

            List<ITestGroup> groups = new List<ITestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(21 * ParameterValidator.VALID_MODULI.Length, groups.Count);
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

            var algSpecs = new AlgSpecs[ParameterValidator.VALID_SIG_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpecs
                {
                    SigType = ParameterValidator.VALID_SIG_GEN_MODES[i],
                    ModuloCapabilities = modCap
                };
            }

            return algSpecs;
        }
    }
}
