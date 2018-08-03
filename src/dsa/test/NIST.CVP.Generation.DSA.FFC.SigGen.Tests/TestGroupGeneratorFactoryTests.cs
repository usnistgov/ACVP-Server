using Moq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetDsaDomainParameters(It.IsAny<DsaDomainParametersParameters>()))
                .Returns(new DsaDomainParametersResult());

            _subject = new TestGroupGeneratorFactory(oracleMock.Object);
        }

        [Test]
        [TestCase(typeof(TestGroupGenerator))]
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
                Algorithm = "DSA",
                Mode = "SigGen",
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
                Mode = "SigGen",
                IsSample = false,
                Capabilities = GetCapabilities(),
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(3 * 7, groups.Count);
        }

        private Capability[] GetCapabilities()
        {
            var caps = new Capability[3];
            caps[0] = new Capability
            {
                L = 2048,
                N = 224,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256,
                HashAlg = ParameterValidator.VALID_HASH_ALGS
            };

            return caps;
        }
    }
}
