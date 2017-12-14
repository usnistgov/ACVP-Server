using System;
using Moq;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.MAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        private TestCaseGenerator _subject;
        private Mock<IRandom800_90> _random;
        private Mock<IHmac> _algo;
        
        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _algo = new Mock<IHmac>();
            _subject = new TestCaseGenerator(_random.Object, _algo.Object);
        }
        
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedGenerate()
        {
            _algo
                .Setup(s => s.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new MacResult("Fail"));

            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionGen()
        {
            _algo
                .Setup(s => s.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());

            
            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateOperation()
        {
            _subject.Generate(new TestGroup(), true);

            _algo.Verify(v => v.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()),
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
            _algo
                .Setup(s => s.Generate(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new MacResult(fakeMac));

            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Message.ToString(), "Message");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Mac.ToString(), "Mac");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }
    }
}
