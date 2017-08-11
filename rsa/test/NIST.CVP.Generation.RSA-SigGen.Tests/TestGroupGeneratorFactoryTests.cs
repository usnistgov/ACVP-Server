using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var caps = new CapabilityObject[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new CapabilityObject
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = (i + 1) * 8
                };
            }

            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                IsSample = false,
                Moduli = new[] { 2048, 3072, 4096 },
                Capabilities = caps,
                SigGenModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" }
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
            var caps = new CapabilityObject[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new CapabilityObject
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = (i + 1) * 8
                };
            }

            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                IsSample = false,
                Moduli = ParameterValidator.VALID_MODULI,
                Capabilities = caps,
                SigGenModes = ParameterValidator.VALID_SIG_GEN_MODES,
            };

            List<ITestGroup> groups = new List<ITestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(63, groups.Count);
        }
    }
}
