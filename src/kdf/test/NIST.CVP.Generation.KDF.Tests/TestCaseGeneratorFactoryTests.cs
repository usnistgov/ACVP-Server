using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.KDF.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                KdfMode = KdfModes.Pipeline,
                MacMode = MacModes.CMAC_TDES,
                TestType = "aft"
            };

            var kdfFactoryMock = new Mock<IKdfFactory>();
            kdfFactoryMock
                .Setup(s => s.GetKdfInstance(It.IsAny<KdfModes>(), It.IsAny<MacModes>(), It.IsAny<CounterLocations>(), It.IsAny<int>()))
                .Returns(new FakeKdf());

            var subject = new TestCaseGeneratorFactory(null, kdfFactoryMock.Object);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(typeof(TestCaseGenerator), generator);
        }
    }
}

