using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHAWrapper.Tests
{
    [TestFixture, UnitTest]
    public class Sha3WrapperTests
    {
        private Mock<ISHA3Factory> _shaFactory;
        private Mock<SHA3Wrapper> _shaWrapper;
        private SHA3.HashFunction _hashFunction;
        private Sha3Wrapper _subject;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new Mock<ISHA3Factory>();
            _shaWrapper = new Mock<SHA3Wrapper>();
            _hashFunction = new SHA3.HashFunction();

            _shaFactory
                .Setup(s => s.GetSHA(It.IsAny<SHA3.HashFunction>()))
                .Returns(_shaWrapper.Object);
            _shaWrapper
                .Setup(s => s.HashMessage(It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<int>(), false))
                .Returns(new BitString(0));

            _subject = new Sha3Wrapper(_shaFactory.Object, _hashFunction);
        }

        [Test]
        public void ShouldCallUnderlyingFactoryMethod()
        {
            _subject.HashMessage(It.IsAny<BitString>());

            _shaFactory.Verify(v => v.GetSHA(It.IsAny<SHA3.HashFunction>()), 
                Times.Once(), 
                nameof(_shaFactory.Object.GetSHA)
            );
        }

        [Test]
        public void ShouldCallUnderlyingShaHashMessage()
        {
            _subject.HashMessage(It.IsAny<BitString>());

            _shaWrapper.Verify(v => v.HashMessage(It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), 
                Times.Once, 
                nameof(_shaWrapper.Object.HashMessage)
            );
        }
    }
}
