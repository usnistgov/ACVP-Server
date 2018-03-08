using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorReseedPredResistTests
    {
        private TestCaseGeneratorReseedPredResist _subject;
        private Mock<IEntropyProviderFactory> _entropyProviderFactory;
        private Mock<IEntropyProvider> _entropyProvider;
        private Mock<IDrbgFactory> _drbgFactory;
        private Mock<IDrbg> _drbg;

        [SetUp]
        public void Setup()
        {
            _entropyProviderFactory = new Mock<IEntropyProviderFactory>();
            _entropyProvider = new Mock<IEntropyProvider>();
            _drbgFactory = new Mock<IDrbgFactory>();
            _drbg = new Mock<IDrbg>();

            _entropyProviderFactory
                .Setup(s => s.GetEntropyProvider(It.IsAny<EntropyProviderTypes>()))
                .Returns(_entropyProvider.Object);
            _entropyProviderFactory
                .Setup(s => s.GetEntropyProvider(EntropyProviderTypes.Testable))
                .Returns(() => new TestableEntropyProvider());

            _entropyProvider.Setup(s => s.AddEntropy(It.IsAny<BitString>()));
            _entropyProvider.Setup(s => s.GetEntropy(0)).Returns(new BitString(0));

            _drbgFactory
                .Setup(s => s.GetDrbgInstance(It.IsAny<DrbgParameters>(), It.IsAny<IEntropyProvider>()))
                .Returns(_drbg.Object);

            _drbg
                .Setup(s => s.Instantiate(It.IsAny<int>(), It.IsAny<BitString>()))
                .Returns(DrbgStatus.Success);
            _drbg
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<BitString>()))
                .Returns(new DrbgResult(new BitString(0)));
            _drbg
                .Setup(s => s.Reseed(It.IsAny<BitString>())).Returns(DrbgStatus.Success);

            _subject = new TestCaseGeneratorReseedPredResist(_entropyProviderFactory.Object, _drbgFactory.Object);
        }

        /// <summary>
        /// For Reseed/PredResist, two generates are run.  
        /// OtherInput should have two objects within it to convey this.
        /// </summary>
        [Test]
        public void ShouldContainThreeCountInOtherInput()
        {
            var vectorSet = DRBG.TestDataMother.GetSampleVectorSet(1, 1, true, true);

            var testGroup = (TestGroup)vectorSet.TestGroups[0];

            var result = _subject.Generate(testGroup, false);

            Assert.AreEqual(2, ((TestCase)result.TestCase).OtherInput.Count);
        }

        [Test]
        [TestCase(true, 2, 2)]
        public void ShouldContainExpectedEntropyWithPrOption(bool predictionResistance, int expectedNonZeroEntropy, int expectedNonZeroAdditionalInput)
        {
            var vectorSet = DRBG.TestDataMother.GetSampleVectorSet(1, 1, true, predictionResistance);

            var testGroup = (TestGroup)vectorSet.TestGroups[0];
            _entropyProvider
                .Setup(s => s.GetEntropy(testGroup.EntropyInputLen))
                .Returns(new BitString(testGroup.EntropyInputLen));
            _entropyProvider
                .Setup(s => s.GetEntropy(testGroup.NonceLen))
                .Returns(new BitString(testGroup.NonceLen));
            _entropyProvider
                .Setup(s => s.GetEntropy(testGroup.PersoStringLen))
                .Returns(new BitString(testGroup.PersoStringLen));
            _entropyProvider
                .Setup(s => s.GetEntropy(testGroup.AdditionalInputLen))
                .Returns(new BitString(testGroup.AdditionalInputLen));

            var result = _subject.Generate(testGroup, false);

            Assert.AreEqual(
                expectedNonZeroEntropy,
                ((TestCase)result.TestCase).OtherInput.Count(c => c.EntropyInput.BitLength > 0),
                nameof(OtherInput.EntropyInput)
            );
            Assert.AreEqual(
                expectedNonZeroAdditionalInput,
                ((TestCase)result.TestCase).OtherInput.Count(c => c.AdditionalInput.BitLength > 0),
                nameof(OtherInput.AdditionalInput)
            );
        }

        [Test]
        public void ShouldInvokeDrbgCorrectly()
        {
            var vectorSet = DRBG.TestDataMother.GetSampleVectorSet(1, 1, true, true);

            var testGroup = (TestGroup)vectorSet.TestGroups[0];

            var result = _subject.Generate(testGroup, false);

            _drbg.Verify(
                v => v.Instantiate(
                    It.IsAny<int>(),
                    It.IsAny<BitString>()
                ),
                Times.Once,
                nameof(_drbg.Object.Instantiate)
            );
            _drbg.Verify(
                v => v.Reseed(
                    It.IsAny<BitString>()
                ),
                Times.Never,
                nameof(_drbg.Object.Reseed)
            );
            _drbg.Verify(
                v => v.Generate(
                    It.IsAny<int>(),
                    It.IsAny<BitString>()
                ),
                Times.Exactly(2),
                nameof(_drbg.Object.Generate)
            );
        }
    }
}