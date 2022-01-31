using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.ParallelHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ParallelHash
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
        [TestCase(new[] { false }, 4)]
        [TestCase(new[] { true }, 4)]
        [TestCase(new[] { true, false }, 8)]
        public async Task ShouldReturnVectorSetWithProperTestGroupsForXOFModes(bool[] xof, int expected)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));

            var blockSize = new MathDomain();
            blockSize.AddSegment(new RangeDomainSegment(null, 1, 128));

            Parameters p = new Parameters
            {
                Algorithm = "ParallelHash",
                DigestSizes = new[] { 128, 256 }.ToList(),
                MessageLength = minMax,
                IsSample = false,
                OutputLength = minMax,
                BlockSize = blockSize,
                XOF = xof
            };

            var groups = new List<TestGroup>();
            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.AreEqual(expected, groups.Count);       // 2 * 2 * 2    (digestsizes * XOF * TestGroups)
        }
    }
}
