using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldReconstituteTestGroupFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assert.IsNotNull(subject);
        }

        [Test]
        public void ShouldSetProperLFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.l, subject.L);
        }

        [Test]
        public void ShouldSetProperNFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.n, subject.N);
        }

        // Answer does not contain Domain Params
        //[Test]
        //public void ShouldSetProperDomainParamsFromDynamicAnswer()
        //{
        //    var sourceAnswer = GetSourceAnswer();
        //    var subject = new TestGroup(sourceAnswer);
        //    Assume.That(subject != null);
        //    Assert.AreEqual(sourceAnswer.p, subject.DomainParams.P);
        //    Assert.AreEqual(sourceAnswer.q, subject.DomainParams.Q);
        //    Assert.AreEqual(sourceAnswer.g, subject.DomainParams.G);
        //}

        [Test]
        public void ShouldSetProperHashAlgFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);

            var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm((string)sourceAnswer.hashAlg);
            Assert.AreEqual(shaAttributes.shaMode, subject.HashAlg.Mode);
            Assert.AreEqual(shaAttributes.shaDigestSize, subject.HashAlg.DigestSize);
        }

        private dynamic GetSourceAnswer()
        {
            // Need sample here
            var sourceVector = new TestVectorSet { IsSample = true, TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
