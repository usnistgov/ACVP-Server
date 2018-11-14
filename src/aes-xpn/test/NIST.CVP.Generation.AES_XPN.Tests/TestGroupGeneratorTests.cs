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
                AadLen = mdAad,
                Algorithm = "AES-XPN",
                IvGen = ivGen,
                IvGenMode = ivGenMode,
                KeyLen = keyLen,
                Direction = mode,
                PayloadLen = mdPt,
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

            Assert.AreEqual(p.AadLen.GetDomainMinMax().Minimum, ((TestGroup)result[0]).AadLength);
        }
        
        [Test]
        public void ShouldSetIvGen()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.IvGen, ((TestGroup)result[0]).IvGeneration);
        }

        [Test]
        public void ShouldSetIvGenMode()
        {
            Parameters p = GetParameters();
            
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(p.IvGenMode, ((TestGroup)result[0]).IvGenerationMode);
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

            Assert.AreEqual(p.PayloadLen.GetDomainMinMax().Minimum, ((TestGroup)result[0]).PayloadLength);
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
                AadLen = new MathDomain().AddSegment(new ValueDomainSegment(1)),
                Algorithm = "test0",
                IvGen = "test",
                IvGenMode = "test2",
                KeyLen = new int[] { 2 },
                Direction = new[] { "test3" },
                PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(3)),
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
