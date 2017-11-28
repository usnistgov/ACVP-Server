using System.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
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
        [TestCase(EccScheme.EphemeralUnified, "ephemeralUnified")]
        [TestCase(EccScheme.FullMqv, "fullMqv")]
        [TestCase(EccScheme.FullUnified, "fullUnified")]
        [TestCase(EccScheme.OnePassDh, "onePassDh")]
        [TestCase(EccScheme.OnePassMqv, "onePassMqv")]
        [TestCase(EccScheme.OnePassUnified, "onePassUnified")]
        [TestCase(EccScheme.StaticUnified, "staticUnified")]
        public void ShouldSetProperTestScheme(EccScheme expectedValue, string value)
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