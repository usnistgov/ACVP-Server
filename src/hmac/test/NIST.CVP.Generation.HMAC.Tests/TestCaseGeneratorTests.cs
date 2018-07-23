using System;
using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
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
        private Mock<IOracle> _oracle;
        
        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _subject = new TestCaseGenerator(_oracle.Object);
        }
        
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedGenerate()
        {
            _oracle
                .Setup(s => s.GetHmacCase(It.IsAny<HmacParameters>()))
                .Throws(new Exception("Fail"));

            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateOperation()
        {
            _subject.Generate(new TestGroup(), true);

            _oracle.Verify(v => v.GetHmacCase(It.IsAny<HmacParameters>()),
                Times.AtLeastOnce,
                $"{nameof(_oracle.Object.GetHmacCase)} should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var key = new BitString("01");
            var message = new BitString("02");
            var tag = new BitString("03");

            _oracle
                .Setup(s => s.GetHmacCase(It.IsAny<HmacParameters>()))
                .Returns(new MacResult()
                {
                    Key = key,
                    Message = message,
                    Tag = tag
                });

            var result = _subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.AreEqual(key, result.TestCase.Key, nameof(key));
            Assert.AreEqual(message, result.TestCase.Message, nameof(message));
            Assert.AreEqual(tag, result.TestCase.Mac, nameof(tag));
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }
    }
}
