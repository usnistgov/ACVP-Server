using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC
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
        public async Task GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = await _subject.GenerateAsync(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public async Task GenerateShouldReturnNullITestCaseOnFailedGenerate()
        {
            _oracle
                .Setup(s => s.GetHmacCaseAsync(It.IsAny<HmacParameters>()))
                .Throws(new Exception("Fail"));

            var result = await _subject.GenerateAsync(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public async Task GenerateShouldInvokeGenerateOperation()
        {
            await _subject.GenerateAsync(new TestGroup(), true);

            _oracle.Verify(v => v.GetHmacCaseAsync(It.IsAny<HmacParameters>()),
                Times.AtLeastOnce,
                $"{nameof(_oracle.Object.GetHmacCaseAsync)} should have been invoked"
            );
        }

        [Test]
        public async Task GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var key = new BitString("01");
            var message = new BitString("02");
            var tag = new BitString("03");

            _oracle
                .Setup(s => s.GetHmacCaseAsync(It.IsAny<HmacParameters>()))
                .Returns(Task.FromResult(new MacResult()
                {
                    Key = key,
                    Message = message,
                    Tag = tag
                }));

            var result = await _subject.GenerateAsync(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.AreEqual(key, result.TestCase.Key, nameof(key));
            Assert.AreEqual(message, result.TestCase.Message, nameof(message));
            Assert.AreEqual(tag, result.TestCase.Mac, nameof(tag));
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }
    }
}
