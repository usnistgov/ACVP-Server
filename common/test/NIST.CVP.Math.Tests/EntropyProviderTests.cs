using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
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
