using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
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
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var result = await subject.ValidateAsync(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldFailIfNDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PubKey.N = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldFailIfPDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.P = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldFailIfQDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.Q = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfDDoesNotMatch()
        {
            var crtForm = false;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            ((PrivateKey)suppliedResult.Key.PrivKey).D = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfDMP1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).DMP1 = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfDMQ1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).DMQ1 = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfIQMPDoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolver(null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).IQMP = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

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
