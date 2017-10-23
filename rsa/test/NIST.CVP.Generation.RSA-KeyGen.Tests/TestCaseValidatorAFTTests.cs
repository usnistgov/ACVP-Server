using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAFTTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch(bool crtForm)
        {
            var testCase = GetTestCase(crtForm);
            var subject = new TestCaseValidatorAFT(testCase);

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
            var subject = new TestCaseValidatorAFT(testCase);

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
            var subject = new TestCaseValidatorAFT(testCase);

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
            var subject = new TestCaseValidatorAFT(testCase);

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
            var subject = new TestCaseValidatorAFT(testCase);

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.D = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result, testCase.Key.PrivKey.D.ToString());
        }

        [Test]
        public void ShouldFailIfDMP1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var subject = new TestCaseValidatorAFT(testCase);

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.DMP1 = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldFailIfDMQ1DoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var subject = new TestCaseValidatorAFT(testCase);

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.DMQ1 = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldFailIfIQMPDoesNotMatch()
        {
            var crtForm = true;
            var testCase = GetTestCase(crtForm);
            var subject = new TestCaseValidatorAFT(testCase);

            var suppliedResult = GetTestCase(crtForm);
            suppliedResult.Key.PrivKey.IQMP = 9001;

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase(bool crtForm = false)
        {
            PrivateKey privateKey;
            if (crtForm)
            {
                privateKey = new PrivateKey
                {
                    DMP1 = 2,
                    DMQ1 = 3,
                    IQMP = 4,
                    P = 5,
                    Q = 6
                };
            }
            else
            {
                privateKey = new PrivateKey
                {
                    D = 4,
                    P = 5,
                    Q = 6
                };
            }

            return new TestCase
            {
                TestCaseId = 1,
                Key = new KeyPair
                {
                    PrivKey = privateKey,
                    PubKey = new PublicKey
                    {
                        E = 7,
                        N = 8
                    }
                }
            };
        }
    }
}
