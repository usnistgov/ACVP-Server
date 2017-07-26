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
            var p = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                HashAlgs = new[] { "SHA-1", "SHA-224" },
                IsSample = false,
                Moduli = new[] { 2048, 3072 },
                N = "123456",
                PrivExp = "123456",
                PubExp = "123456",
                Salt = "123456",
                SaltLen = new[] { 24, 28 },
                SaltMode = "fixed",
                SigGenModes = new[] { "ansx9.31", "pss" }
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
                HashAlgs = new[] { "SHA-1", "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA-512/224", "SHA-512/256" },
                IsSample = false,
                Moduli = new[] { 2048, 3072, 4096 },
                N = "123456",
                PrivExp = "123456",
                PubExp = "123456",
                Salt = "123456",
                SaltLen = new[] { 24, 28, 32, 36, 40, 44, 48 },
                SaltMode = "fixed",
                SigGenModes = new[] { "ansx9.31", "pss", "pkcs1v15" }
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
