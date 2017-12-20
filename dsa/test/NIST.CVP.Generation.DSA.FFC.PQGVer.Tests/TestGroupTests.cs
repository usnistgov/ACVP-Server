using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests
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
        public void ShouldSetProperTestTypeFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.testType, subject.TestType);
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

        [Test]
        public void ShouldSetProperHashAlgFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.hashAlg, subject.HashAlg.Name);
        }

        [Test]
        public void ShouldSetProperGGenModeFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer("g");
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual((string)sourceAnswer.gMode, EnumHelpers.GetEnumDescriptionFromEnum(subject.GGenMode));
        }

        [Test]
        public void ShouldSetProperPQGenModeFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual((string)sourceAnswer.pqMode, EnumHelpers.GetEnumDescriptionFromEnum(subject.PQGenMode));
        }

        private dynamic GetSourceAnswer(string mode = "pq")
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups(mode).Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
