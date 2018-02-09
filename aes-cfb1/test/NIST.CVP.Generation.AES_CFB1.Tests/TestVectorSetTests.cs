using System;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
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

        // @@@ possible to get strong typing out of projection?

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
                Assert.IsTrue(!string.IsNullOrEmpty(test.iv.ToString()), nameof(test.iv));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
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
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.iv.ToString()), nameof(test.iv));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
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
        public void EncryptShouldIncludeCipherTextInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.cipherText.ToString()));
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
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.plainText.ToString()));
            }
        }

        [Test]
        public void EncryptShouldIncludeCipherTextInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(item.cipherText.ToString()));
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
            foreach (var item in results)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => item.plainText.ToString());
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
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.plainText.ToString()));
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
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.cipherText.ToString()));
            }
        }

        [Test]
        public void DecryptShouldIncludePlainTextInResultProjectionWhenNotFailureTest()
        {
            var subject = GetSubject(1, "decrypt", false);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(item.plainText.ToString()));
            }
        }
        
        [Test]
        public void DecryptShouldExcludePlainTextInResultProjectionWhenFailureTest()
        {
            var subject = GetSubject(1, "decrypt", true);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assume.That(item.decryptFail);
                Assert.Throws(typeof(RuntimeBinderException), () => item.plainText.ToString());
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
            foreach (var item in results)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => item.cipherText.ToString());
            }
        }

        [Test]
        public void MCTEncryptShouldIncludeIVKeyPlainTextAndCipherTextInResultArrayWithinAnswerProjection()
        {
            var subject = GetMCTSubject(1, "encrypt");
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                foreach (var result in test.resultsArray)
                {
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.iv.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.key.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.plainText.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.cipherText.ToString()));
                }
            }
        }

        [Test]
        public void MCTDecryptShouldIncludeIVKeyPlainTextAndCipherTextInResultArrayWithinAnswerProjection()
        {
            var subject = GetMCTSubject(1, "decrypt");
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                foreach (var result in test.resultsArray)
                {
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.iv.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.key.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.plainText.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.cipherText.ToString()));
                }
            }
        }

        [Test]
        public void MCTEncryptShouldIncludeIVKeyAndPlainTextInResultArrayWithinPromptProjection()
        {
            var subject = GetMCTSubject(1, "encrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.iv.ToString()));
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.key.ToString()));
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.plainText.ToString()));
            }
        }

        [Test]
        public void MCTDecryptShouldIncludeIVKeyAndCipherTextInResultArrayWithinPromptProjection()
        {
            var subject = GetMCTSubject(1, "decrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.iv.ToString()));
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.key.ToString()));
                Assert.IsTrue((bool) !string.IsNullOrEmpty(test.cipherText.ToString()));
            }
        }

        [Test]
        public void MCTEncryptShouldExcludeCipherTextInResultArrayWithinPromptProjection()
        {
            var subject = GetMCTSubject(1, "encrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.cipherText.ToString());
            }
        }

        [Test]
        public void MCTDecryptShouldExcludePlainTextInResultArrayWithinPromptProjection()
        {
            var subject = GetMCTSubject(1, "decrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.plainText.ToString());
            }
        }

        [Test]
        public void MCTEncryptShouldIncludeIvKeyPlainTextAndCipherTextInResultProjection()
        {
            var subject = GetMCTSubject(1, "encrypt");
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                foreach (var result in item.resultsArray)
                {
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.iv.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.key.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.plainText.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.cipherText.ToString()));
                }
            }
        }

        [Test]
        public void MCTDecryptShouldIncludeIvKeyPlainTextAndCipherTextInResultProjection()
        {
            var subject = GetMCTSubject(1, "decrypt");
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                foreach (var result in item.resultsArray)
                {
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.iv.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.key.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.plainText.ToString()));
                    Assert.IsTrue((bool) !string.IsNullOrEmpty(result.cipherText.ToString()));
                }
            }
        }

        private TestVectorSet GetSubject(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var subject = new TestVectorSet {Algorithm = "AES-CBC"};
            var testGroups = _tdm.GetTestGroups(groups, direction, failureTest);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }

        private TestVectorSet GetMCTSubject(int groups = 1, string direction = "encrypt")
        {
            var subject = new TestVectorSet { Algorithm = "AES-CBC" };
            var testGroups = _tdm.GetMCTTestGroups(groups, direction);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
