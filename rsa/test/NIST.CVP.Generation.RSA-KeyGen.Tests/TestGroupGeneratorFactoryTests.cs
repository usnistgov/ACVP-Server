using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorFactory();
        }

        [Test]
        [TestCase(typeof(TestGroupGeneratorAlgorithmFunctionalTest))]
        [TestCase(typeof(TestGroupGeneratorGeneratedDataTest))]
        [TestCase(typeof(TestGroupGeneratorKnownAnswerTests))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators();

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            var result = _subject.GetTestGroupGenerators();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators();
            Parameters p = new Parameters
                {
                    Algorithm = "RSA",
                    Mode = "KeyGen",
                    FixedPubExp = "010001",
                    InfoGeneratedByServer = true,
                    IsSample = false,
                    PubExpMode = "fixed",
                    AlgSpecs = BuildCapabilities()
                };

            List<ITestGroup> groups = new List<ITestGroup>();

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
            Parameters p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                InfoGeneratedByServer = true,
                IsSample = false,
                PubExpMode = "random",
                AlgSpecs = BuildCapabilities()
            };

            List<ITestGroup> groups = new List<ITestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(68, groups.Count);
        }

        private AlgSpec[] BuildCapabilities()
        {
            var caps = new Capability[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new Capability
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashAlgs = ParameterValidator.VALID_HASH_ALGS,
                    PrimeTests = ParameterValidator.VALID_PRIME_TESTS
                };
            }

            var algSpecs = new AlgSpec[ParameterValidator.VALID_KEY_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpec
                {
                    RandPQ = ParameterValidator.VALID_KEY_GEN_MODES[i],
                    Capabilities = caps
                };
            }

            return algSpecs;
        }
    }
}
