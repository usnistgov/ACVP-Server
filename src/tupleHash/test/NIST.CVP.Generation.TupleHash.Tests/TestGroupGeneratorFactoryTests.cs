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
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainTwoGenerators()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count() == 2);
        }

        [Test]
        [TestCase(false, 4)]
        [TestCase(true, 8)]
        public void ShouldReturnVectorSetWithProperTestGroupsForXOFModes(bool xof, int expected)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));
            var minMaxMsg = new MathDomain();
            minMaxMsg.AddSegment(new RangeDomainSegment(null, 0, 65536));

            Parameters p = new Parameters
            {
                Algorithm = "TupleHash",
                DigestSizes = new[] { 128, 256 },
                MessageLength = minMaxMsg,
                IsSample = false,
                OutputLength = minMax,
                XOF = xof
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(genny.BuildTestGroups(p));
            }

            Assert.AreEqual(expected, groups.Count);       // 2 * 2 * 2    (digestsizes * XOF * TestGroups)
        }
    }
}
