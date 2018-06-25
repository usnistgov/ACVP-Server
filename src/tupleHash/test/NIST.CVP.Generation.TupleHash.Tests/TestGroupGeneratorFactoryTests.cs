using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
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
        [TestCase(typeof(TestGroupGeneratorAlgorithmFunctional))]
        [TestCase(typeof(TestGroupGeneratorMonteCarlo))]
        [TestCase(typeof(TestGroupGeneratorVariableOutput))]
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
        [TestCase(false, 6)]
        [TestCase(true, 12)]
        public void ShouldReturnVectorSetWithProperTestGroupsForXOFModes(bool xof, int expected)
        {
            var result = _subject.GetTestGroupGenerators();

            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));

            Parameters p = new Parameters
            {
                Algorithm = "TupleHash",
                DigestSizes = new[] { 128, 256 },
                BitOrientedInput = true,
                BitOrientedOutput = true,
                IncludeNull = true,
                IsSample = false,
                OutputLength = minMax,
                XOF = xof
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(expected, groups.Count);       // 2 * 2 * 3    (digestsizes * XOF * TestGroups)
        }
    }
}
