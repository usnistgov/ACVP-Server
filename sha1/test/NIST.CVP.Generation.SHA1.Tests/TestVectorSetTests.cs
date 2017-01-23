using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
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
            Assert.IsTrue(!string.IsNullOrEmpty(group.msgLen.ToString()), nameof(group.msgLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.digLen.ToString()), nameof(group.digLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                // No message
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.digest.ToString()), nameof(test.digest));
                Assert.IsTrue(!string.IsNullOrEmpty(test.deferred.ToString()), nameof(test.deferred));
                Assert.IsTrue(!string.IsNullOrEmpty(test.failureTest.ToString()), nameof(test.failureTest));
            }
        }

        [Test]
        public void ShouldContainElementsWithinPromptProjection()
        {
            var subject = GetSubject();
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.msgLen.ToString()), nameof(group.msgLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.digLen.ToString()), nameof(group.digLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                // No digest
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.message.ToString()), nameof(test.message));
            }
        }

        [Test]
        public void ShouldContainElementsWithinResultProjection()
        {
            var subject = GetSubject();
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.tcId.ToString()), nameof(item.tcId));
            }
        }

        [Test]
        public void HashShouldIncludeDigestInAnswerProjection()
        {
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.digest.ToString()));
            }
        }

        [Test]
        public void HashShouldIncludeMessageInPromptProjection()
        {
            var subject = GetSubject();
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.message.ToString()));
            }
        }

        [Test]
        public void HashShouldIncludeDigestInResultProjection()
        {
            var subject = GetSubject();
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.digest.ToString()));
            }
        }

        [Test]
        public void HashShouldExcludeMessageInAnswerProjection()
        {
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.message.ToString());
            }
        }

        [Test]
        public void HashShouldExcludeDigestInPromptProjection()
        {
            var subject = GetSubject();
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.digest.ToString());
            }
        }

        [Test]
        public void HashShouldExcludeMessageInResultProjection()
        {
            var subject = GetSubject();
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => item.message.ToString());
            }
        }

        private TestVectorSet GetSubject(int groups = 1, bool failureTest = false)
        {
            var subject = new TestVectorSet {Algorithm = "SHA1"};
            var testGroups = _tdm.GetTestGroups(groups, failureTest);
            subject.TestGroups = testGroups.Select(g => (ITestGroup) g).ToList();
            return subject;
        }
    }
}
