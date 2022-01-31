using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.SigGen
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetEddsaKeyAsync(It.IsAny<EddsaKeyParameters>()))
                .Returns(Task.FromResult(new EddsaKeyResult { Key = new EdKeyPair(new BitString("ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf"), new BitString("BEEF")) }));

            _subject = new TestGroupGeneratorFactory(oracleMock.Object);
        }

        private TestGroupGeneratorFactory _subject;

        [Test]
        [TestCase(typeof(TestGroupGenerator))]
        [TestCase(typeof(TestGroupGeneratorBitFlip))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainOneGenerator()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var p = new Parameters
            {
                Algorithm = "EDDSA",
                Mode = "SigGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var p = new Parameters
            {
                Algorithm = "EDDSA",
                Mode = "SigGen",
                IsSample = false,
                Curve = ParameterValidator.VALID_CURVES,
                PreHash = true
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.AreEqual(8, groups.Count);
        }
    }
}
