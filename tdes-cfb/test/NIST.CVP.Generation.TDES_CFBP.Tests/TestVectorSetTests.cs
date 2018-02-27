using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Generation.Core;
using NUnit.Framework;
using System;
using System.Linq;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
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
            Assert.AreEqual(2, results.Count);

            foreach (var group in results)
            {
                Assert.AreEqual(15, group.tests.Count);
            }
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
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.direction.ToString()), nameof(group.direction));

            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key1.ToString()), nameof(test.key1));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key2.ToString()), nameof(test.key2));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key3.ToString()), nameof(test.key3));
                Assert.IsTrue(!string.IsNullOrEmpty(test.deferred.ToString()), nameof(test.deferred));
                Assert.IsTrue(!string.IsNullOrEmpty(test.failureTest.ToString()), nameof(test.failureTest));
            }
        }

        [Test]
        public void ShouldContainElementsWithinPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.direction.ToString()), nameof(group.direction));

            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key1.ToString()), nameof(test.key1), !string.IsNullOrEmpty(test.key2.ToString()), nameof(test.key2), !string.IsNullOrEmpty(test.key3.ToString()), nameof(test.key3));
            }
        }

        [Test]
        public void ShouldContainElementsWithinResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(group.tgId.ToString()), nameof(group.tgId));
            }
        }

        [Test]
        public void EncryptShouldIncludeCipherTextInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.ct.ToString()));
            }
        }

        [Test]
        public void EncryptShouldIncludePlainTextInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.pt.ToString()));
            }
        }

        [Test]
        public void EncryptShouldIncludeCipherTextInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(test.ct.ToString()));
                }
            }
        }

        [Test]
        public void EncryptShouldExcludePlainTextInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.pt.ToString());
            }
        }

        [Test]
        public void EncryptShouldExcludeCipherTextInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.ct.ToString());
            }
        }

        [Test]
        public void EncryptShouldExcludePlainTextInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assert.Throws(typeof(RuntimeBinderException), () => test.pt.ToString());
                }
            }
        }

        [Test]
        public void DecryptShouldIncludePlainTextInAnswerProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.pt.ToString()));
            }
        }

        [Test]
        public void DecryptShouldIncludeCipherTextInPromptProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.ct.ToString()));
            }
        }

        [Test]
        public void DecryptShouldIncludePlainTextInResultProjectionWhenNotFailureTest()
        {
            var subject = GetSubject(1, "decrypt", null, false);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(test.pt.ToString()));
                }
            }
        }

        [Test]
        public void DecryptShouldExcludePlainTextInResultProjectionWhenFailureTest()
        {
            var subject = GetSubject(1, "decrypt", null, true);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assume.That(test.decryptFail);
                    Assert.Throws(typeof(RuntimeBinderException), () => test.pt.ToString());
                }
            }
        }

        [Test]
        public void DecryptShouldExcludeCipherTextInAnswerProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.ct.ToString());
            }
        }

        [Test]
        public void DecryptShouldExcludePlainTextInPromptProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.pt.ToString());
            }
        }

        [Test]
        public void DecryptShouldExcludeCipherTextInResultProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assert.Throws(typeof(RuntimeBinderException), () => test.ct.ToString());
                }
            }
        }

        private TestVectorSet GetSubject(int groups = 1, string direction1 = "encrypt", string direction2 = "decrypt", bool failureTest = false)
        {
            var subject = new TestVectorSet { Algorithm = "TDES-CFB1" };
            var testGroups = _tdm.GetTestGroups(groups, direction1, direction2, failureTest);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
