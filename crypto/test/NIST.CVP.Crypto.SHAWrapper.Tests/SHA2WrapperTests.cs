using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHAWrapper.Tests
{
    [TestFixture, UnitTest]
    public class SHA2WrapperTests
    {
        private Mock<ISHAFactory> _shaFactory;
        private Mock<ISHABase> _shaBase;
        private SHA2.HashFunction _hashFunction;
        private Sha2Wrapper _subject;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new Mock<ISHAFactory>();
            _shaBase = new Mock<ISHABase>();
            _hashFunction = new SHA2.HashFunction();

            _shaFactory
                .Setup(s => s.GetSHA(It.IsAny<SHA2.HashFunction>()))
                .Returns(_shaBase.Object);
            _shaBase
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new BitString(0));
            
            _subject = new Sha2Wrapper(_shaFactory.Object, _hashFunction);
        }

        [Test]
        public void ShouldCallUnderlyingFactoryMethod()
        {
            _subject.HashMessage(It.IsAny<BitString>());
            
            _shaFactory.Verify(v => v.GetSHA(It.IsAny<SHA2.HashFunction>()), 
                Times.Once(), 
                nameof(_shaFactory.Object.GetSHA)
            );
        }
        
        [Test]
        public void ShouldCallUnderlyingShaHashMessage()
        {
            _subject.HashMessage(It.IsAny<BitString>());

            _shaBase.Verify(v => v.HashMessage(It.IsAny<BitString>()), 
                Times.Once, 
                nameof(_shaBase.Object.HashMessage)
            );
        }
    }
}
