﻿using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftNoKdfNoKcTests
    {
        private TestCaseValidatorAftNoKdfNoKc _subject;
        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>>();
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public void ShouldSucceedValidation(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);
            
            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(new KasResult(testCase.Z, testCase.HashZ));

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Passed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public void ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];
            testCase.EphemeralPublicKeyIut = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public void ShouldFailWhenIutDoesNotProvideStaticKeyPair(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];
            testCase.StaticPublicKeyIut = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public void ShouldFailWhenIutDoesNotProvideHashZ(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];
            
            testCase.HashZ = null;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public void ShouldFailWhenMismatchedHashZ(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];

            BitString newValue = testCase.HashZ.GetDeepCopy();
            newValue[0] += 2;

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(new KasResult(testCase.Z, newValue));

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }
        
        private TestGroup GetData(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = TestDataMother.GetTestGroups(1, true, "aft").TestGroups[0];
            testGroup.KasMode = KasMode.NoKdfNoKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;

            return testGroup;
        }
    }
}