using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldFailIfNDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PubKey.N = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldFailIfPDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.P = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldFailIfQDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.Q = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldFailIfDDoesNotMatch()
        {
            var crtForm = false;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            ((PrivateKey)suppliedResult.Key.PrivKey).D = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldFailIfDMP1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).DMP1 = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldFailIfDMQ1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).DMQ1 = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldFailIfIQMPDoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null, null, null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).IQMP = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase(bool crtForm = false)
        {
            var pubKey = new PublicKey
            {
                E = 1,
                N = 2
            };

            PrivateKeyBase privateKey;
            if (crtForm)
            {
                privateKey = new CrtPrivateKey
                {
                    DMP1 = 3,
                    DMQ1 = 4,
                    IQMP = 5,
                    P = 6,
                    Q = 7
                };
            }
            else
            {
                privateKey = new PrivateKey
                {
                    D = 8,
                    P = 9,
                    Q = 10
                };
            }

            return new TestCase
            {
                TestCaseId = 1,
                Deferred = false,
                Key = new KeyPair
                {
                    PrivKey = privateKey,
                    PubKey = pubKey
                }
            };
        }

        private TestGroup GetTestGroup(bool crtForm)
        {
            return new TestGroup
            {
                KeyFormat = crtForm ? PrivateKeyModes.Crt : PrivateKeyModes.Standard
            };
        }
    }
}
