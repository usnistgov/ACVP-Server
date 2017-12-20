using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv1.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv1.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                AuthenticationMethod = AuthenticationMethods.Dsa,
                HashAlg =  new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                TestType = "aft"
            };

            var kdfFactoryMock = new Mock<IIkeV1Factory>();
            kdfFactoryMock
                .Setup(s => s.GetIkeV1Instance(It.IsAny<AuthenticationMethods>(), It.IsAny<HashFunction>()))
                .Returns(new DsaIkeV1(null));

            var subject = new TestCaseGeneratorFactory(null, kdfFactoryMock.Object);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(typeof(TestCaseGenerator), generator);
        }
    }
}
