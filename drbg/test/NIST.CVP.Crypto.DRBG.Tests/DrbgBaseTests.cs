using Moq;
using Moq.Protected;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Crypto.DRBG.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture]
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
        public void ShouldInvokeAlgorithmSetSecurityStrengthsOnSuccess()
        {
            _entropy
                .Setup(s => s.GetEntropy(_parameters.EntropyInputLen))
                .Returns(new BitString(_parameters.EntropyInputLen));
            _entropy
                .Setup(s => s.GetEntropy(_parameters.NonceLen))
                .Returns(new BitString(_parameters.NonceLen));

            _subject.Object.Instantiate(_parameters.SecurityStrength, new BitString(_parameters.PersoStringLen));
            _subject.Protected().Verify("SetSecurityStrengths", Times.Once(), _parameters.SecurityStrength);
        }

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
            _subject.Object.SetMaxPersonalizationStringLength(1);

            var result = _subject.Object.Instantiate(_parameters.SecurityStrength, new BitString(_parameters.PersoStringLen));

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
            _subject.Object.SetMaxNumberOfBitsPerRequest(1);

            var result = _subject.Object.Generate(_parameters.ReturnedBitsLen, new BitString(_parameters.AdditionalInputLen));

            Assert.AreEqual(DrbgStatus.Error, result.DrbgStatus);
        }

        [Test]
        public void ShouldReturnErrorIfAdditionalInputExceedsMax()
        {
            _subject.Object.SetMaxAdditionalInput(1);

            var result = _subject.Object.Generate(_parameters.ReturnedBitsLen, new BitString(_parameters.AdditionalInputLen));

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
