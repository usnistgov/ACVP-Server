using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Math;

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
        [TestCase(typeof(TestGroupGeneratorAft))]
        [TestCase(typeof(TestGroupGeneratorGdt))]
        [TestCase(typeof(TestGroupGeneratorKat))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                FixedPubExp = new BitString("010001"),
                InfoGeneratedByServer = true,
                IsSample = false,
                PubExpMode = PublicExponentModes.Fixed,
                KeyFormat = PrivateKeyModes.Standard,
                AlgSpecs = BuildCapabilities()
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {

            var result = _subject.GetTestGroupGenerators(new Parameters());
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                InfoGeneratedByServer = true,
                IsSample = false,
                PubExpMode = PublicExponentModes.Random,
                KeyFormat = PrivateKeyModes.Crt,
                AlgSpecs = BuildCapabilities()
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(100, groups.Count);
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
                    PrimeTests = new [] {PrimeTestFips186_4Modes.TblC2, PrimeTestFips186_4Modes.TblC3}
                };
            }

            var algSpecs = new []
            {
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B32,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B33,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B34,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B35,
                    Capabilities = caps
                }, 
                new AlgSpec
                {
                    RandPQ = PrimeGenFips186_4Modes.B36,
                    Capabilities = caps
                } 
            };

            return algSpecs;
        }
    }
}
