using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv2.Tests
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
            Assert.IsTrue(!string.IsNullOrEmpty(group.nInitLength.ToString()), nameof(group.nInitLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.nRespLength.ToString()), nameof(group.nRespLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.dhLength.ToString()), nameof(group.dhLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.derivedKeyingMaterialLength.ToString()), nameof(group.derivedKeyingMaterialLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.hashAlg.ToString()), nameof(group.hashAlg));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.sKeySeed.ToString()), nameof(test.sKeySeed));
                Assert.IsTrue(!string.IsNullOrEmpty(test.derivedKeyingMaterial.ToString()), nameof(test.derivedKeyingMaterial));
                Assert.IsTrue(!string.IsNullOrEmpty(test.derivedKeyingMaterialChild.ToString()), nameof(test.derivedKeyingMaterialChild));
                Assert.IsTrue(!string.IsNullOrEmpty(test.derivedKeyingMaterialDh.ToString()), nameof(test.derivedKeyingMaterialDh));
                Assert.IsTrue(!string.IsNullOrEmpty(test.sKeySeedReKey.ToString()), nameof(test.sKeySeedReKey));
            }
        }

        [Test]
        public void ShouldContainElementsWithinPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            Assert.IsTrue(!string.IsNullOrEmpty(group.nInitLength.ToString()), nameof(group.nInitLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.nRespLength.ToString()), nameof(group.nRespLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.dhLength.ToString()), nameof(group.dhLength));
            Assert.IsTrue(!string.IsNullOrEmpty(group.hashAlg.ToString()), nameof(group.hashAlg));
            Assert.IsTrue(!string.IsNullOrEmpty(group.derivedKeyingMaterialLength.ToString()), nameof(group.derivedKeyingMaterialLength));
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                Assert.IsTrue(!string.IsNullOrEmpty(test.nInit.ToString()), nameof(test.nInit));
                Assert.IsTrue(!string.IsNullOrEmpty(test.nResp.ToString()), nameof(test.nResp));
                Assert.IsTrue(!string.IsNullOrEmpty(test.spiInit.ToString()), nameof(test.spiInit));
                Assert.IsTrue(!string.IsNullOrEmpty(test.spiResp.ToString()), nameof(test.spiResp));
                Assert.IsTrue(!string.IsNullOrEmpty(test.gir.ToString()), nameof(test.gir));
                Assert.IsTrue(!string.IsNullOrEmpty(test.girNew.ToString()), nameof(test.girNew));
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

                foreach (var test in group.tests)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(test.tcId.ToString()), nameof(test.tcId));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.sKeySeed.ToString()), nameof(test.sKeySeed));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.derivedKeyingMaterial.ToString()), nameof(test.derivedKeyingMaterial));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.derivedKeyingMaterialChild.ToString()), nameof(test.derivedKeyingMaterialChild));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.derivedKeyingMaterialDh.ToString()), nameof(test.derivedKeyingMaterialDh));
                    Assert.IsTrue(!string.IsNullOrEmpty(test.sKeySeedReKey.ToString()), nameof(test.sKeySeedReKey));
                }
            }
        }

        [Test]
        public void ShouldIncludeSKeySeedInAnswerProjection()
        {
            var subject = GetSubject(1);
            var results = subject.AnswerProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.sKeySeed.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeNInitInPromptProjection()
        {
            var subject = GetSubject(1);
            var results = subject.PromptProjection;
            var group = results[0];
            var tests = group.tests;
            foreach (var test in tests)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(test.nInit.ToString()));
            }
        }

        [Test]
        public void ShouldIncludeSKeySeedInResultProjection()
        {
            var subject = GetSubject(1);
            var results = subject.ResultProjection;
            foreach (var group in results)
            {
                foreach (var test in group.tests)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(test.sKeySeed.ToString()));
                }
            }
        }

        private TestVectorSet GetSubject(int groups = 1)
        {
            var subject = new TestVectorSet { Algorithm = "kdf-components", Mode = "ikev2" };
            var testGroups = _tdm.GetTestGroups(groups);
            subject.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return subject;
        }
    }
}
