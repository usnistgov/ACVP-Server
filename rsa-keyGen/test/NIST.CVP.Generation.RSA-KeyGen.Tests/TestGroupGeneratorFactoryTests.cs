using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
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
                    Algorithm = "RSA-KeyGen",
                    HashAlgs = new[] { "SHA-1", "SHA-224" },
                    FixedPubExp = "010001",
                    InfoGeneratedByServer = true,
                    IsSample = false,
                    KeyGenModes = new[] { "B.3.2", "B.3.3" },
                    Moduli = new[] { 2048, 3072 },
                    PrimeTests = new[] { "tblC2" },
                    PubExpMode = "fixed"
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
                Algorithm = "RSA-KeyGen",
                HashAlgs = new[] { "SHA-1", "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA-512/224", "SHA-512/256" },
                InfoGeneratedByServer = true,
                IsSample = false,
                KeyGenModes = new[] { "B.3.2", "B.3.3", "B.3.4", "B.3.5", "B.3.6" },
                Moduli = new[] { 2048, 3072, 4096 },
                PrimeTests = new[] { "tblC2", "tblC3" },
                PubExpMode = "random"
            };

            List<ITestGroup> groups = new List<ITestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(102, groups.Count);
        }
    }
}
