using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.Tests
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
        [TestCase(
            "test",
            new string[] { "encrypt", "decrypt" },
            new int[] { 128, 256 },
            new int[] { 0, 128, 256, 120 },
            "",
            "",
            new int[] { 0, 128 },
            new int[] { 64, 128 }
        )]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string label,
            string[] mode,
            int[] keyLen,
            int[] ptLen,
            string ivGen,
            string ivGenMode,
            int[] aadLen,
            int[] tagLen
        )
        {

            MathDomain mdPt = GetMathDomainFromArray(ptLen);
            MathDomain mdAad = GetMathDomainFromArray(aadLen);
            MathDomain mdTag = GetMathDomainFromArray(tagLen);

            Parameters p = new Parameters()
            {
                aadLen = mdAad,
                Algorithm = "AES-XPN",
                ivGen = ivGen,
                ivGenMode = ivGenMode,
                KeyLen = keyLen,
                Direction = mode,
                PtLen = mdPt,
                TagLen = mdTag
            };
            int expectedResultCount = aadLen.Length * keyLen.Length * mode.Length * ptLen.Length * tagLen.Length;

            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(expectedResultCount, result.Count());
        }

        [Test]
        public void ShouldSetAadLen()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.aadLen.GetDomainMinMax().Minimum, ((TestGroup)result[0]).AADLength);
        }
        
        [Test]
        public void ShouldSetIvGen()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.ivGen, ((TestGroup)result[0]).IVGeneration);
        }

        [Test]
        public void ShouldSetIvGenMode()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.ivGenMode, ((TestGroup)result[0]).IVGenerationMode);
        }

        [Test]
        public void ShouldSetDirection()
        {
            Parameters p = GetParameters();

            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.Direction[0], ((TestGroup)result[0]).Function);
        }

        [Test]
        public void ShouldSetPtLen()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.PtLen.GetDomainMinMax().Minimum, ((TestGroup)result[0]).PTLength);
        }

        [Test]
        public void ShouldSetTagLen()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.TagLen.GetDomainMinMax().Minimum, ((TestGroup)result[0]).TagLength);
        }

        [Test]
        public void ShouldSetSaltGen()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.SaltGen, ((TestGroup)result[0]).SaltGen);
        }

        private Parameters GetParameters()
        {
            return new Parameters()
            {
                aadLen = new MathDomain().AddSegment(new ValueDomainSegment(1)),
                Algorithm = "test0",
                ivGen = "test",
                ivGenMode = "test2",
                KeyLen = new int[] { 2 },
                Direction = new[] { "test3" },
                PtLen = new MathDomain().AddSegment(new ValueDomainSegment(3)),
                TagLen = new MathDomain().AddSegment(new ValueDomainSegment(64)),
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
