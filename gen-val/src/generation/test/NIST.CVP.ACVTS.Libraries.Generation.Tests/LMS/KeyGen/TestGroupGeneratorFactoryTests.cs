using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.KeyGen
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
        [TestCase(typeof(TestGroupGenerator))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainOneGenerator()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var capabilities = ParameterBuilder.GetGeneralCapabilitiesWith(
                ParameterValidator.VALID_LMS_TYPES, ParameterValidator.VALID_LMOTS_TYPES);

            var p = new Parameters
            {
                Algorithm = "LMS",
                Mode = "KeyGen",
                IsSample = false,
                Capabilities = capabilities
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroupsAsync(p).Result);
            }

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var capabilities = ParameterBuilder.GetGeneralCapabilitiesWith(
                ParameterValidator.VALID_LMS_TYPES, ParameterValidator.VALID_LMOTS_TYPES);

            var p = new Parameters
            {
                Algorithm = "LMS",
                Mode = "KeyGen",
                IsSample = false,
                Capabilities = capabilities
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroupsAsync(p).Result);
            }

            Assert.AreEqual(74, groups.Count);
        }
    }
}
