using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.Tests
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
        public void ShouldSetProperKdrFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.kdr, subject.Kdr);
        }

        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "1");
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("keylength")]
        [TestCase("KEYLENGTH")]
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "128");
            Assert.IsTrue(result);
            Assert.AreEqual(128, subject.AesKeyLength);
        }

        private dynamic GetSourceAnswer()
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
