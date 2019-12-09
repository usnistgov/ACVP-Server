using System;
using Moq;
using Moq.Protected;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.DRBG.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture,  FastCryptoTest]
    public class DrbgBaseTests
    {
        private Mock<FakeDrbgImplementation> _subject;
        private Mock<IEntropyProvider> _entropy;
        private DrbgParameters _parameters;

        [SetUp]
        public void Setup()
        {
            _entropy = new Mock<IEntropyProvider>();

            _parameters = new DrbgParameters();
            _parameters.Mechanism = DrbgMechanism.Counter;
            _parameters.Mode = DrbgMode.AES128;
            _parameters.NonceLen = 64;
            _parameters.EntropyInputLen = 128;
            _parameters.PersoStringLen = 128;
            _parameters.AdditionalInputLen = 128;
            _parameters.SecurityStrength = 128;
            _parameters.DerFuncEnabled = true;
            _parameters.ReseedImplemented = true;
            _parameters.PredResistanceEnabled = true;
            _parameters.ReturnedBitsLen = 128 * 8;

            _subject = new Mock<FakeDrbgImplementation>(_entropy.Object, _parameters);
            _subject.CallBase = true;
        }

        #region Instantiate
        [Test]
        public void ShouldInvokeAlgorithmInstantiateOnSuccess()
        {
            _entropy
                .Setup(s => s.GetEntropy(_parameters.EntropyInputLen))
                .Returns(new BitString(_parameters.EntropyInputLen));
            _entropy
                .Setup(s => s.GetEntropy(_parameters.NonceLen))
                .Returns(new BitString(_parameters.NonceLen));

            _subject.Object.Instantiate(_parameters.SecurityStrength, new BitString(_parameters.PersoStringLen));
            _subject.Protected().Verify("InstantiateAlgorithm", Times.Once(), ItExpr.IsAny<BitString>(), ItExpr.IsAny<BitString>(), ItExpr.IsAny<BitString>());
        }

        [Test]
        public void ShouldReturnRequestedSecurityStrengthToHighWhenAboveMax()
        {
            var result = _subject.Object.Instantiate(257, new BitString(_parameters.PersoStringLen));

            Assert.AreEqual(DrbgStatus.RequestedSecurityStrengthTooHigh, result);
        }

        [Test]
        public void ShouldReturnPersonalizationStringTooLongWhenPersoStringExceedsMax()
        {
            var result = _subject.Object.Instantiate(_parameters.SecurityStrength, new BitString(Int32.MaxValue));

            Assert.AreEqual(DrbgStatus.PersonalizationStringTooLong, result);
        }

        [Test]
        public void ShouldReturnCatastrophicErrorWhenEntropyLengthDoesNotMeetExpectation()
        {
            _entropy
                .Setup(s => s.GetEntropy(_parameters.EntropyInputLen))
                .Returns(new BitString(_parameters.EntropyInputLen + 1));

            var result = _subject.Object.Instantiate(_parameters.SecurityStrength, new BitString(_parameters.PersoStringLen));

            Assert.AreEqual(DrbgStatus.CatastrophicError, result);
        }

        [Test]
        [TestCase(DrbgStatus.Success)]
        [TestCase(DrbgStatus.Error)]
        public void ShouldReturnStatusOfInstantiateAlgorithm(DrbgStatus statusToReturn)
        {
            _entropy
                .Setup(s => s.GetEntropy(_parameters.EntropyInputLen))
                .Returns(new BitString(_parameters.EntropyInputLen));

            _subject.Object.DrbgStatus = statusToReturn;
            var result = _subject.Object.Instantiate(_parameters.SecurityStrength, new BitString(_parameters.PersoStringLen));

            Assert.AreEqual(statusToReturn, result);
        }
        #endregion Instantiate

        #region Reseed
        [Test]
        public void ShouldCallAlgorithmReseedOnReseed()
        {
            _entropy
                .Setup(s => s.GetEntropy(_parameters.EntropyInputLen))
                .Returns(new BitString(_parameters.EntropyInputLen));

            _subject.Object.Reseed(new BitString(_parameters.AdditionalInputLen));
            _subject.Protected().Verify("ReseedAlgorithm", Times.Once(), ItExpr.IsAny<BitString>(), ItExpr.IsAny<BitString>());
        }

        [Test]
        [TestCase(DrbgStatus.Success)]
        [TestCase(DrbgStatus.Error)]
        public void ShouldReturnStatusOfReseedAlgorithm(DrbgStatus statusToReturn)
        {
            _entropy
                .Setup(s => s.GetEntropy(_parameters.EntropyInputLen))
                .Returns(new BitString(_parameters.EntropyInputLen));

            _subject.Object.DrbgStatus = statusToReturn;
            var result = _subject.Object.Reseed(new BitString(_parameters.AdditionalInputLen));

            Assert.AreEqual(statusToReturn, result);
        }
        #endregion Reseed

        #region Generate
        [Test]
        public void ShouldCallAlgorithmGenerate()
        {
            _subject.Object.Generate(_parameters.ReturnedBitsLen, new BitString(_parameters.AdditionalInputLen));
            _subject.Protected().Verify("GenerateAlgorithm", Times.Once(), ItExpr.IsAny<int>(), ItExpr.IsAny<BitString>());
        }

        [Test]
        public void ShouldReturnSuccessAlgorithmGenerate()
        {
            DrbgResult dr = new DrbgResult(new BitString(0));
            _subject.Object.DrbgResult = dr;

            var result = _subject.Object.Generate(_parameters.ReturnedBitsLen, new BitString(_parameters.AdditionalInputLen));

            Assert.AreEqual(DrbgStatus.Success, result.DrbgStatus);
        }

        [Test]
        public void ShouldReturnErrorIfRequestedNumberOfBitsExceedsMaximum()
        {
            var result = _subject.Object.Generate(Int32.MaxValue, new BitString(_parameters.AdditionalInputLen));

            Assert.AreEqual(DrbgStatus.Error, result.DrbgStatus);
        }

        [Test]
        public void ShouldReturnErrorIfAdditionalInputExceedsMax()
        {
            var result = _subject.Object.Generate(_parameters.ReturnedBitsLen, new BitString(Int32.MaxValue));

            Assert.AreEqual(DrbgStatus.Error, result.DrbgStatus);
        }

        [Test]
        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public void ShouldCallReseedWhenPredictionResistance(bool predictionResistance, int numberOfTimesToReseed)
        {
            _parameters.PredResistanceEnabled = predictionResistance;

            _subject.Object.Generate(_parameters.ReturnedBitsLen, new BitString(_parameters.AdditionalInputLen));
            _subject.Protected().Verify("ReseedAlgorithm", Times.Exactly(numberOfTimesToReseed), ItExpr.IsAny<BitString>(), ItExpr.IsAny<BitString>());
        }
        #endregion Generate

    }
}
