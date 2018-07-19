using System;
using Moq;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.KMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMVTTests
    {
        private TestCaseGeneratorMVT _subject;
        private Mock<IRandom800_90> _random;
        private Mock<IKmac> _algo;

        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _algo = new Mock<IKmac>();
            _subject = new TestCaseGeneratorMVT(_random.Object, _algo.Object);
        }

        [Test]
        public void GeneratorShouldWantToGenerate15Cases()
        {
            Assert.AreEqual(15, _subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = _subject.Generate(new TestGroup
            {
                MacLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 32, 65536))
            }, false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedGenerate()
        {
            _algo
                .Setup(s => s.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new MacResult("Fail"));

            var result = _subject.Generate(new TestGroup
            {
                MacLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 32, 65536))
            }, false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionGen()
        {
            _algo
                .Setup(s => s.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());


            var result = _subject.Generate(new TestGroup
            {
                MacLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 32, 65536))
            }, false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateOperation()
        {
            _subject.Generate(new TestGroup
            {
                MacLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 32, 65536))
            }, false);

            _algo.Verify(v => v.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<string>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                $"{nameof(_algo.Object.Generate)} should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeMac = new BitString(new byte[] { 1 });
            var fakeMsg = new BitString(new byte[] { 2 });
            _random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            _random
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(5);
            _random
                .Setup(s => s.GetRandomAlphaCharacters(It.IsAny<int>()))
                .Returns("ABC");
            _algo
                .Setup(s => s.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new MacResult(fakeMac));

            var result = _subject.Generate(new TestGroup
            {
                MacLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 32, 65536))
            }, false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty((result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty((result.TestCase).Message.ToString(), "Message");
            Assert.IsNotEmpty((result.TestCase).Mac.ToString(), "Mac");
            Assert.IsNotEmpty((result.TestCase).Customization, "Customization");
            Assert.IsNotEmpty((result.TestCase).MacVerified.ToString(), "Mac Verification");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }
    }
}
