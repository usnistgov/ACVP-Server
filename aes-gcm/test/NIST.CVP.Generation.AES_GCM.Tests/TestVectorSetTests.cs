using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.Tests
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

            Random800_90 rand = new Random800_90();

            foreach (var testGroup in prompts.TestGroups)
            {
                testGroup.Tests.Clear();
            }

            Assert.Throws(
                Is.TypeOf<Exception>()
                    .And.Message.EqualTo("Could not reconstitute TestVectorSet from supplied answers and prompts"),
                () => new TestVectorSet(answers.ToDynamic(), prompts.ToDynamic()));
        }

        // @@@ possible to get strong typing out of projection?

        [Test]
        public void ShouldContainElementsWithinAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.direction.ToString()), nameof(group.direction));
            Assert.IsTrue(!string.IsNullOrEmpty(group.testType.ToString()), nameof(group.testType));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ivGen.ToString()), nameof(group.ivGen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ivGenMode.ToString()), nameof(group.ivGenMode));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ivLen.ToString()), nameof(group.ivLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ptLen.ToString()), nameof(group.ptLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.aadLen.ToString()), nameof(group.aadLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.tagLen.ToString()), nameof(group.tagLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.tag.ToString()), nameof(test.tag));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
                Assert.IsTrue(!string.IsNullOrEmpty(test.aad.ToString()), nameof(test.aad));
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
            Assert.IsTrue(!string.IsNullOrEmpty(group.testType.ToString()), nameof(group.testType));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ivGen.ToString()), nameof(group.ivGen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ivGenMode.ToString()), nameof(group.ivGenMode));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ivLen.ToString()), nameof(group.ivLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.ptLen.ToString()), nameof(group.ptLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.aadLen.ToString()), nameof(group.aadLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.tagLen.ToString()), nameof(group.tagLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.keyLen.ToString()), nameof(group.keyLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.key.ToString()), nameof(test.key));
                Assert.IsTrue(!string.IsNullOrEmpty(test.aad.ToString()), nameof(test.aad));
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
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.cipherText.ToString()));
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
                Assert.Throws(typeof(RuntimeBinderException), () => test.plainText.ToString());
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
        public void EncryptShouldExcludeTagInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            foreach (var item in results)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => item.tag.ToString());
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
        public void DecryptShouldIncludeTagInPromptProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tag.ToString()));
            }
        }

        [Test]
        public void DecryptShouldIncludePlainTextInResultProjectionWhenNotFailureTest()
        {
            var subject = GetSubject(1, "decrypt", false);
            var results = subject.ResultProjection;
            foreach (var item in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.plainText.ToString()));
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
        public void DecryptShouldExcludeCipherTextInAnswerProjection()
        {
            var subject = GetSubject(1, "decrypt");
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.cipherText.ToString());
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

        private TestVectorSet GetSubject(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var subject = new TestVectorSet {Algorithm = "AES-GCM"};
            var testGroups = _tdm.GetTestGroups(groups, direction, failureTest);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
