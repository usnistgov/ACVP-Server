using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SSH.Tests
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
            foreach (var groups in results)
            {
                Assert.AreEqual(15, groups.tests.Count);
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
            var subject = GetSubject();
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.cipher.ToString()), nameof(group.cipher));
            Assert.IsTrue(!string.IsNullOrEmpty(group.hashAlg.ToString()), nameof(group.hashAlg));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.initialIvServer.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.encryptionKeyServer.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.integrityKeyServer.ToString()));

                Assert.IsTrue(!string.IsNullOrEmpty(test.initialIvClient.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.encryptionKeyClient.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.integrityKeyClient.ToString()));
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
                Assert.IsTrue(!string.IsNullOrEmpty(item.initialIvServer.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.encryptionKeyServer.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.integrityKeyServer.ToString()));

                Assert.IsTrue(!string.IsNullOrEmpty(item.initialIvClient.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.encryptionKeyClient.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(item.integrityKeyClient.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeKHAndSessionIdInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.cipher.ToString()), nameof(group.cipher));
            Assert.IsTrue(!string.IsNullOrEmpty(group.hashAlg.ToString()), nameof(group.hashAlg));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.k.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.h.ToString()));
                Assert.IsTrue(!string.IsNullOrEmpty(test.sessionId.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeClientAndServerResultsInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(group.tgId.ToString()), nameof(group.tgId));

                foreach (var test in group.tests)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(test.initialIvServer.ToString()));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.encryptionKeyServer.ToString()));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.integrityKeyServer.ToString()));

                    Assert.IsTrue(!string.IsNullOrEmpty(test.initialIvClient.ToString()));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.encryptionKeyClient.ToString()));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.integrityKeyClient.ToString()));
                }
            }
        }

        private TestVectorSet GetSubject(int groups = 1)
        {
            var subject = new TestVectorSet { Algorithm = "kdf-components", Mode = "ssh" };
            var testGroups = _tdm.GetTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
