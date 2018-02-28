using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KDF.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            var fakeFactory = new Mock<IKdfFactory>();
            fakeFactory
                .Setup(s => s.GetKdfInstance(It.IsAny<KdfModes>(), It.IsAny<MacModes>(), It.IsAny<CounterLocations>(), It.IsAny<int>()))
                .Returns(new FakeKdf());
            _subject = new TestCaseValidatorFactory(fakeFactory.Object);
        }

        [Test]
        public void ShouldReturnCorrectValidatorType()
        {
            var testVectorSet = new TestVectorSet
            {
                TestGroups = new List<ITestGroup>
                {
                    new TestGroup
                    {
                        Tests = new List<ITestCase>
                        {
                            new TestCase()
                        }
                    }
                }
            };

            var result = _subject.GetValidators(testVectorSet);

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(typeof(TestCaseValidator), result.First());
        }
    }
}
