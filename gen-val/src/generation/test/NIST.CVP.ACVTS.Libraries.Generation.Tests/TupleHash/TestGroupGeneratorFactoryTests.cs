using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.TupleHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TupleHash
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

            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainTwoGenerators()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.That(result.Count() == 2, Is.True);
        }

        [Test]
        [TestCase(false, 4)]
        [TestCase(true, 4)]
        public async Task ShouldReturnVectorSetWithProperTestGroupsForXOFModes(bool xof, int expected)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));
            var minMaxMsg = new MathDomain();
            minMaxMsg.AddSegment(new RangeDomainSegment(null, 0, 65536));

            Parameters p = new Parameters
            {
                Algorithm = "TupleHash",
                DigestSizes = new List<int> { 128, 256 },
                MessageLength = minMaxMsg,
                IsSample = false,
                OutputLength = minMax,
                XOF = new[] { xof }
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.That(groups.Count, Is.EqualTo(expected));       // 2 * 2 * 2    (digestsizes * XOF * TestGroups)
        }
    }
}
