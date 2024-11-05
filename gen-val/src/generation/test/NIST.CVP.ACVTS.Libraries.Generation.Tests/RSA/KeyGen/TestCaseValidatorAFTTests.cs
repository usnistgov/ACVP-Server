using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen
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
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var result = await subject.ValidateAsync(testCase);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldFailIfNDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PubKey.N = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldFailIfPDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.P = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldFailIfQDoesNotMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.Q = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldFailIfDDoesNotMatch()
        {
            var crtForm = false;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            ((PrivateKey)suppliedResult.Key.PrivKey).D = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldFailIfDMP1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).DMP1 = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldFailIfDMQ1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).DMQ1 = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldFailIfIQMPDoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var testGroup = GetTestGroup(crtForm);
            var subject = new TestCaseValidatorAft(testCase, testGroup, new DeferredTestCaseResolverAFT(null));

            var suppliedResult = GetTestCase(crtForm);
            ((CrtPrivateKey)suppliedResult.Key.PrivKey).IQMP = 9001;

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
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
