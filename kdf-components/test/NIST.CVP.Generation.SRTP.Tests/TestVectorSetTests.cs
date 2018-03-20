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
            var subject = new TestVectorSet(source);
            Assert.AreEqual(2, subject.TestGroups.Count);
        }

        [Test]
        public void ShouldContainElementsWithinAnswerProjection()
        {
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.kdr.ToString()), nameof(group.kdr));
            Assert.IsTrue(!string.IsNullOrEmpty(group.aesKeyLength.ToString()), nameof(group.aesKeyLength));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.srtpKe.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.srtpKa.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.srtpKs.ToString()));

                Assert.IsTrue(!string.IsNullOrEmpty(test.srtcpKe.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.srtcpKa.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.srtcpKs.ToString()));
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
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtpKe.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtpKa.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtpKs.ToString()));

                Assert.IsTrue(!string.IsNullOrEmpty(item.srtcpKe.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtcpKa.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtcpKs.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeQuestionInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.kdr.ToString()), nameof(group.kdr));
            Assert.IsTrue(!string.IsNullOrEmpty(group.aesKeyLength.ToString()), nameof(group.aesKeyLength));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.masterKey.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.masterSalt.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.index.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.srtcpIndex.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeResultsInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtpKe.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtpKa.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtpKs.ToString()));

                Assert.IsTrue(!string.IsNullOrEmpty(item.srtcpKe.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtcpKa.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.srtcpKs.ToString()));
            }
        }

        private TestVectorSet GetSubject(int groups = 1)
        {
            var subject = new TestVectorSet { Algorithm = "kdf-components", Mode = "srtp" };
            var testGroups = _tdm.GetTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
