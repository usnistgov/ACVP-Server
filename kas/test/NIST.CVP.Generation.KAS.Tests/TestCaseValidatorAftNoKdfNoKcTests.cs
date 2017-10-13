using System;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftNoKdfNoKcTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private TestCaseValidatorAftNoKdfNoKc _subject;
        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>> _deferredResolver;
        

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>>();
        }

        [Test]
        public void ShouldSucceedValidation()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);
            
            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(new KasResult(testCase.Z, testCase.HashZ));

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("passed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        [TestCase(FfcScheme.DhEphem)]
        public void ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme)
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];
            testGroup.Scheme = FfcScheme.DhEphem;
            testCase.EphemeralPublicKeyIut = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ShouldFailWhenIutDoesNotProvideHashZ()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];
            
            testCase.HashZ = null;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ShouldFailWhenMismatchedHashZ()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            BitString newValue = testCase.HashZ.GetDeepCopy();
            newValue[0] += 2;

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(new KasResult(testCase.Z, newValue));

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }
        
        private TestGroup GetData()
        {
            return _tdm.GetTestGroups().First();
        }
    }
}