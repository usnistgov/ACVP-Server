using Moq;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Entropy
{
    [TestFixture, UnitTest]
    public class EntropyProviderTests
    {
        [Test]
        public void ShouldCallRandomFunction()
        {
            Mock<IRandom800_90> mock = new Mock<IRandom800_90>();
            mock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(1));
            EntropyProvider subject = new EntropyProvider(mock.Object);

            var result = subject.GetEntropy(1);

            mock.Verify(v => v.GetRandomBitString(It.IsAny<int>()), Times.Once, nameof(mock.Object.GetRandomBitString));
        }
    }
}
