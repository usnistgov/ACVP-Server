using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private TestGroup _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroup();
        }
        
        [Test]
        public void ShouldReconstituteTestGroupFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            _subject = new TestGroup(sourceAnswer);
            Assert.IsNotNull(_subject);
        }

        [Test]
        [TestCase(FfcScheme.DhEphem, "dhEphem")]
        [TestCase(FfcScheme.DhHybrid1, "dhHybrid1")]
        [TestCase(FfcScheme.DhHybridOneFlow, "dhHybridOneFlow")]
        [TestCase(FfcScheme.DhStatic, "dhStatic")]
        [TestCase(FfcScheme.Mqv1, "mqv1")]
        [TestCase(FfcScheme.DhOneFlow, "dhOneFlow")]
        [TestCase(FfcScheme.Mqv2, "mqv2")]
        public void ShouldSetProperTestScheme(FfcScheme expectedValue, string value)
        {
            var sourceAnswer = GetSourceAnswer();

            sourceAnswer.scheme = value;

            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(expectedValue, subject.Scheme);
        }

        [Test]
        [TestCase("VAL")]
        [TestCase("AFT")]
        public void ShouldSetProperTestType(string expectedValue)
        {
            var sourceAnswer = GetSourceAnswer();

            sourceAnswer.testType = expectedValue;

            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(expectedValue, subject.TestType);
        }

        [Test]
        [TestCase(KeyAgreementRole.InitiatorPartyU, "initiator")]
        [TestCase(KeyAgreementRole.ResponderPartyV, "responder")]
        public void ShouldSetProperTestKasRole(KeyAgreementRole expectedValue, string value)
        {
            var sourceAnswer = GetSourceAnswer();

            sourceAnswer.kasRole = value;

            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(expectedValue, subject.KasRole);
        }

        [Test]
        [TestCase(KasMode.NoKdfNoKc, "noKdfNoKc")]
        [TestCase(KasMode.KdfNoKc, "kdfNoKc")]
        [TestCase(KasMode.KdfKc, "kdfKc")]
        public void ShouldSetProperTestKasMode(KasMode expectedValue, string value)
        {
            var sourceAnswer = GetSourceAnswer();

            sourceAnswer.kasMode = value;

            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(expectedValue, subject.KasMode);
        }

        [Test]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, "SHA2-224")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "SHA2-256")]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, "SHA2-384")]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, "SHA2-512")]
        public void ShouldSetProperTestHashAlg(ModeValues mode, DigestSizes size, string value)
        {
            var sourceAnswer = GetSourceAnswer();

            sourceAnswer.hashAlg = value;

            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(mode, subject.HashAlg.Mode);
            Assert.AreEqual(size, subject.HashAlg.DigestSize);
        }

        private dynamic GetSourceAnswer()
        {
            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}