using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.Tests
{
   [TestFixture, UnitTest]
    public class TestVectorSetTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldHaveTheExpectedAnswerProjection()
        {
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(15, results[0].tests.Count);
        }

        [Test]
        public void ShouldHaveTheExpectedPromptProjection()
        {
            var subject = GetSubject();
            var results = subject.PromptProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(15, results[0].tests.Count);
        }

        [Test]
        public void ShouldHaveTheExpectedResultProjection()
        {
            var subject = GetSubject(2);
            var results = subject.ResultProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(30, results.Count);
        }

        [Test]
        public void ShouldReconstituteTestVectorFromAnswerAndPrompt()
        {
            var source = GetSubject(2).ToDynamic();
            var subject = new TestVectorSet(source, source);
            Assert.AreEqual(2, subject.TestGroups.Count);
        }

        [Test]
        public void ShouldFailToReconstituteTestVectorSetWhenNotMatched()
        {
            var answers = GetSubject();
            var prompts = GetSubject();

            foreach (var testGroup in prompts.TestGroups)
            {
                testGroup.Tests.Clear();
            }

            Assert.Throws(
                Is.TypeOf<Exception>()
                    .And.Message.EqualTo("Could not reconstitute TestVectorSet from supplied answers and prompts"),
                () => new TestVectorSet(answers.ToDynamic(), prompts.ToDynamic()));
        }

        [Test]
        public void ShouldContainElementsWithinAnswerProjection()
        {
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.sharedInfoLength.ToString()), nameof(group.sharedInfoLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.hashAlg.ToString()), nameof(group.hashAlg));
            Assert.IsTrue(!string.IsNullOrEmpty(group.fieldSize.ToString()), nameof(group.fieldSize));
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyDataLength.ToString()), nameof(group.keyDataLength));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.z.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.sharedInfo.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.keyData.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeResultsInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var item in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.keyData.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeQuestionInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.sharedInfoLength.ToString()), nameof(group.sharedInfoLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.hashAlg.ToString()), nameof(group.hashAlg));
            Assert.IsTrue(!string.IsNullOrEmpty(group.fieldSize.ToString()), nameof(group.fieldSize));
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyDataLength.ToString()), nameof(group.keyDataLength));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.z.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.sharedInfo.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeResultsInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.keyData.ToString()));
            }
        }

        private TestVectorSet GetSubject(int groups = 1)
        {
            var subject = new TestVectorSet { Algorithm = "kdf-components", Mode = "ansix9.63" };
            var testGroups = _tdm.GetTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }

}
