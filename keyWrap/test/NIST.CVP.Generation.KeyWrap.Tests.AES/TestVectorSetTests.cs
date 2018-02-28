using System;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests.AES
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
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
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
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
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
                Assert.IsTrue(!string.IsNullOrEmpty(test.cipherText.ToString()));
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
                Assert.IsTrue(!string.IsNullOrEmpty(test.plainText.ToString()));
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
                    Assert.IsTrue(!string.IsNullOrEmpty(test.cipherText.ToString()));
                }
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
                Assert.Throws(typeof(RuntimeBinderException), () => test.cipherText.ToString());
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
                    Assert.Throws(typeof(RuntimeBinderException), () => test.plainText.ToString());
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
                Assert.IsTrue(!string.IsNullOrEmpty(test.plainText.ToString()));
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
                Assert.IsTrue(!string.IsNullOrEmpty(test.cipherText.ToString()));
            }
        }

        [Test]
        public void DecryptShouldIncludePlainTextInResultProjectionWhenNotFailureTest()
        {
            var subject = GetSubject(1, "decrypt", false);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(test.plainText.ToString()));
                }
            }
        }

        [Test]
        public void DecryptShouldExcludePlainTextInResultProjectionWhenFailureTest()
        {
            var subject = GetSubject(1, "decrypt", true);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assume.That(test.decryptFail);
                    Assert.Throws(typeof(RuntimeBinderException), () => test.plainText.ToString());
                }
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
                Assert.Throws(typeof(RuntimeBinderException), () => test.plainText.ToString());
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
                    Assert.Throws(typeof(RuntimeBinderException), () => test.cipherText.ToString());
                }
            }
        }
        
        private TestVectorSetBase<TestGroup> GetSubject(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var subject = new TestVectorSet { Algorithm = "KeyWrap" };
            var testGroups = _tdm.GetTestGroups(groups, direction, failureTest);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}