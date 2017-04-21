using System;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
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
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            //Assert.IsTrue(!string.IsNullOrEmpty(group.function.ToString()), nameof(group.function));
            Assert.IsTrue(!string.IsNullOrEmpty(group.digestSize.ToString()), nameof(group.digestSize));
            //Assert.IsTrue(!string.IsNullOrEmpty(group.bitOriented.ToString()), nameof(group.bitOriented));
            //Assert.IsTrue(!string.IsNullOrEmpty(group.includeNull.ToString()), nameof(group.includeNull));

            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.md.ToString()), nameof(test.md));
            }
        }

        [Test]
        public void ShouldContainElementsWithinPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            //Assert.IsTrue(!string.IsNullOrEmpty(group.function.ToString()), nameof(group.function));

            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.msg.ToString()), nameof(test.msg));
            }
        }

        [Test]
        public void ShouldContainElementsWithinResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.tcId.ToString()), nameof(item.tcId));
            }
        }

        [Test]
        public void HashShouldIncludeDigestInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.md.ToString()));
            }
        }

        [Test]
        public void HashShouldIncludeMessageInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.msg.ToString()));
            }
        }

        [Test]
        public void HashShouldIncludeDigestInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.md.ToString()));
            }
        }

        //[Test]
        public void HashShouldExcludeMessageInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.msg.ToString());
            }
        }

        [Test]
        public void HashShouldExcludeDigestInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.md.ToString());
            }
        }

        //[Test]
        public void HashShouldExcludeMessageInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => item.msg.ToString());
            }
        }

        [Test]
        public void MCTHashShouldIncludeMessageAndDigestInResultArrayWithinAnswerProjection()
        {
            var subject = GetMCTSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                foreach (var result in test.resultsArray)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(result.msg.ToString()));
                    Assert.IsTrue(!string.IsNullOrEmpty(result.md.ToString()));
                }
            }
        }

        //[Test]
        //public void MCTHashShouldIncludeMessageInResultArrayWithinPromptProjection()
        //{
        //    var subject = GetMCTSubject(1);
        //    var results = subject.PromptProjection;
        //    var group = results[0];
        //    var tests = group.tests;
        //    foreach (var test in tests)
        //    {
        //        foreach (var result in test.resultsArray)
        //        {
        //            Assert.IsTrue(!string.IsNullOrEmpty(result.message.ToString()));
        //        }
        //    }
        //}

        //[Test]
        //public void MCTHashShouldExcludeDigestInResultArrayWithinPromptProjection()
        //{
        //    var subject = GetMCTSubject(1);
        //    var results = subject.PromptProjection;
        //    var group = results[0];
        //    var tests = group.tests;
        //    foreach (var test in tests)
        //    {
        //        foreach (var result in test.resultsArray)
        //        {
        //            Assert.Throws(typeof(RuntimeBinderException), () => result.digest.ToString());
        //        }
        //    }
        //}

        [Test]
        public void MCTHashShouldIncludeMessageAndDigestInResultProjection()
        {
            var subject = GetMCTSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                foreach (var result in item.resultsArray)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(result.msg.ToString()));
                    Assert.IsTrue(!string.IsNullOrEmpty(result.md.ToString()));
                }
            }
        }

        private TestVectorSet GetMCTSubject(int groups = 1)
        {
            var subject = new TestVectorSet {Algorithm = "SHA"};
            var testGroups = _tdm.GetMCTTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup) g).ToList();
            return subject;
        }

        private TestVectorSet GetSubject(int groups = 1, bool failureTest = false)
        {
            var hashFunction = new HashFunction()
            {
                Mode = ModeValues.SHA2,
                DigestSize = DigestSizes.d224
            };

            var subject = new TestVectorSet {Algorithm = "SHA2"};
            var testGroups = _tdm.GetTestGroups(hashFunction, groups, failureTest);
            subject.TestGroups = testGroups.Select(g => (ITestGroup) g).ToList();
            return subject;
        }
    }
}
