using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
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
        [TestCase("Function", "sha3")]
        [TestCase("testType", "MCT")]
        public void ShouldReturnSetStringName(string name, string value)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnFalseIfMergeFails()
        {
            var testCase = new TestCase { Message = null, Digest = null, TestCaseId = 42 };
            var testCases = new List<ITestCase> { testCase };
            var tg = new TestGroup { Tests = testCases };

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

        private dynamic GetSourceAnswer()
        {
            var hashFunction = new HashFunction
            {
                Capacity = 448,
                DigestSize = 224,
                XOF = false
            };

            var sourceVector = new TestVectorSet
            {
                TestGroups = _tdm.GetTestGroups(hashFunction).Select(g => (ITestGroup)g).ToList()
            };

            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
