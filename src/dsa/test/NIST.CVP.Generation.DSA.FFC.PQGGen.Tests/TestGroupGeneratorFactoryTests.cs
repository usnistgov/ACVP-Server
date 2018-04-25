using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
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
        [TestCase(typeof(TestGroupGeneratorG))]
        [TestCase(typeof(TestGroupGeneratorPQ))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators();
            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainTwoGenerators()
        {
            var result = _subject.GetTestGroupGenerators();
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators();

            var p = new Parameters
            {
                Algorithm = "DSA",
                Mode = "PQGGen",
                IsSample = false,
                Capabilities = GetCapabilities(),
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
            var result = _subject.GetTestGroupGenerators();

            var p = new Parameters
            {
                Algorithm = "DSA",
                Mode = "PQGGen",
                IsSample = false,
                Capabilities = GetCapabilities(),
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual((6 * 2 * 2) + (4 * 2 * 2) + (4 * 2 * 2), groups.Count);
        }

        private Capability[] GetCapabilities()
        {
            var caps = new Capability[3];
            caps[0] = new Capability
            {
                L = 2048,
                N = 224,
                GGen = ParameterValidator.VALID_G_MODES,
                PQGen = ParameterValidator.VALID_PQ_MODES,
                HashAlgs = ParameterValidator.VALID_HASH_ALGS
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256,
                GGen = ParameterValidator.VALID_G_MODES,
                PQGen = ParameterValidator.VALID_PQ_MODES,
                HashAlgs = ParameterValidator.VALID_HASH_ALGS.Where(h => h != "sha2-224" && h != "sha2-512/224").ToArray()
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256,
                GGen = ParameterValidator.VALID_G_MODES,
                PQGen = ParameterValidator.VALID_PQ_MODES,
                HashAlgs = ParameterValidator.VALID_HASH_ALGS.Where(h => h != "sha2-224" && h != "sha2-512/224").ToArray()
            };

            return caps;
        }
    }
}
