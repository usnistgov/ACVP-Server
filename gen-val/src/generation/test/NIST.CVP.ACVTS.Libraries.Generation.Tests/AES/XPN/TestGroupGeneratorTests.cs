using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XPN
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGenerator();
        }

        [Test]
        public async Task ShouldSetAadLen()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).AadLength, Is.EqualTo(p.AadLen.GetDomainMinMax().Minimum));
        }

        [Test]
        public async Task ShouldSetIvGen()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).IvGeneration, Is.EqualTo(p.IvGen));
        }

        [Test]
        public async Task ShouldSetIvGenMode()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).IvGenerationMode, Is.EqualTo(p.IvGenMode));
        }

        [Test]
        public async Task ShouldSetDirection()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).Function, Is.EqualTo(p.Direction[0]));
        }

        [Test]
        public async Task ShouldSetPtLen()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).PayloadLength, Is.EqualTo(p.PayloadLen.GetDomainMinMax().Minimum));
        }

        [Test]
        public async Task ShouldSetTagLen()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).TagLength, Is.EqualTo(64));
        }

        [Test]
        public async Task ShouldSetSaltGen()
        {
            Parameters p = GetParameters();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(((TestGroup)result[0]).SaltGen, Is.EqualTo(p.SaltGen));
        }

        private Parameters GetParameters()
        {
            return new Parameters()
            {
                AadLen = new MathDomain().AddSegment(new ValueDomainSegment(1)),
                Algorithm = "test0",
                IvGen = "test",
                IvGenMode = "test2",
                KeyLen = new int[] { 2 },
                Direction = new[] { "test3" },
                PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(3)),
                TagLen = new[] { 64 },
                IsSample = false,
                SaltGen = "test4"
            };
        }

        private MathDomain GetMathDomainFromArray(int[] values)
        {
            MathDomain md = new MathDomain();

            foreach (var value in values)
            {
                md.AddSegment(new ValueDomainSegment(value));
            }

            return md;
        }
    }
}
