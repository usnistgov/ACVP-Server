using System;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NIST.CVP.Generation.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests
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
            Assert.AreEqual(1, results[0].tests.Count);
        }

        [Test]
        public void ShouldHaveTheExpectedPromptProjection()
        {
            var subject = GetSubject();
            var results = subject.PromptProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].tests.Count);
        }

        [Test]
        public void ShouldHaveTheExpectedResultProjection()
        {
            var subject = GetSubject(2);
            var results = subject.ResultProjection;
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
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

        // @@@ possible to get strong typing out of projection?

        [Test]
        public void ShouldContainElementsWithinAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.testType.ToString()), nameof(group.testType));
            Assert.IsTrue(!string.IsNullOrEmpty(group.derFunc.ToString()), nameof(group.derFunc));
            Assert.IsTrue(!string.IsNullOrEmpty(group.predResistance.ToString()), nameof(group.predResistance));
            Assert.IsTrue(!string.IsNullOrEmpty(group.entropyInputLen.ToString()), nameof(group.entropyInputLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.reSeed.ToString()), nameof(group.reSeed));
            Assert.IsTrue(!string.IsNullOrEmpty(group.nonceLen.ToString()), nameof(group.nonceLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.persoStringLen.ToString()), nameof(group.persoStringLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.additionalInputLen.ToString()), nameof(group.additionalInputLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.returnedBitsLen.ToString()), nameof(group.returnedBitsLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.entropyInput.ToString()), nameof(test.entropyInput));
                Assert.IsTrue(!string.IsNullOrEmpty(test.nonce.ToString()), nameof(test.nonce));
                Assert.IsTrue(!string.IsNullOrEmpty(test.persoString.ToString()), nameof(test.persoString));

                Assert.IsTrue(!string.IsNullOrEmpty(test.returnedBits.ToString()), nameof(test.returnedBits));
            }
        }

        [Test]
        public void ShouldContainElementsWithinPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.testType.ToString()), nameof(group.testType));
            Assert.IsTrue(!string.IsNullOrEmpty(group.derFunc.ToString()), nameof(group.derFunc));
            Assert.IsTrue(!string.IsNullOrEmpty(group.predResistance.ToString()), nameof(group.predResistance));
            Assert.IsTrue(!string.IsNullOrEmpty(group.entropyInputLen.ToString()), nameof(group.entropyInputLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.reSeed.ToString()), nameof(group.reSeed));
            Assert.IsTrue(!string.IsNullOrEmpty(group.nonceLen.ToString()), nameof(group.nonceLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.persoStringLen.ToString()), nameof(group.persoStringLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.additionalInputLen.ToString()), nameof(group.additionalInputLen));
            Assert.IsTrue(!string.IsNullOrEmpty(group.returnedBitsLen.ToString()), nameof(group.returnedBitsLen));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.entropyInput.ToString()), nameof(test.entropyInput));
                Assert.IsTrue(!string.IsNullOrEmpty(test.nonce.ToString()), nameof(test.nonce));
                Assert.IsTrue(!string.IsNullOrEmpty(test.persoString.ToString()), nameof(test.persoString));
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
                Assert.IsTrue(!string.IsNullOrEmpty(item.returnedBits.ToString()), nameof(item.returnedBits));
            }
        }
        
        [Test]
        public void PromptProjectionShouldNotContainReturnedBits()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.Throws(typeof(RuntimeBinderException), () => test.returnedBits.ToString());
            }
        }
        
        private TestVectorSet GetSubject(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var subject = new TestVectorSet {Algorithm = "AES-ECB"};
            var testGroups = _tdm.GetTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
