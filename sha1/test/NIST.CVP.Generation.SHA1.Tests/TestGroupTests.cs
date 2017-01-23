using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
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
        public void ShouldSetProperMessageLengthFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.msgLen, subject.MessageLength);
        }

        [Test]
        public void ShouldSetProperDigestLengthFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.digLen, subject.DigestLength);
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
        [TestCase("Fredo")]
        [TestCase("A5")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnparsableValues(string value)
        {
            var subject = new TestGroup();
            var result = subject.SetString("msglen", value);
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnFalseIfMergeFails()
        {
            Random800_90 rand = new Random800_90();
            var testCase = new TestCase()
            {
                Message = rand.GetRandomBitString(8),
                Digest = rand.GetRandomBitString(8),
                TestCaseId = 42
            };

            List<ITestCase> testCases = new List<ITestCase> { testCase };
            TestGroup tg = new TestGroup() { Tests = testCases };

            var result = tg.MergeTests(testCases);

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
        [TestCase("MsgLen")]
        [TestCase("MSGLEN")]
        public void ShouldSetMessageLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.MessageLength);
        }

        [Test]
        [TestCase("DigLen")]
        [TestCase("DIGLEN")]
        public void ShouldSetDigestLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.DigestLength);
        }

        private dynamic GetSourceAnswer()
        {
            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
