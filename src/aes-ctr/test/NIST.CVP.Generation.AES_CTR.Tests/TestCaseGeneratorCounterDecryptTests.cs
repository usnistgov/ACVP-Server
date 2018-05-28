using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorCounterDecryptTests
    {
        private TestCaseGeneratorCounterDecrypt _subject;

        private Mock<IRandom800_90> _random;
        private Mock<IBlockCipherEngine> _engine;
        private Mock<IBlockCipherEngineFactory> _engineFactory;
        private Mock<IModeBlockCipher<SymmetricCounterResult>> _mode;
        private Mock<IModeBlockCipherFactory> _modeFactory;
        private Mock<ICounter> _counter;
        private Mock<ICounterFactory> _counterFactory;

        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            _engine = new Mock<IBlockCipherEngine>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(_engine.Object);
            _mode = new Mock<IModeBlockCipher<SymmetricCounterResult>>();
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(() => new SymmetricCounterResult(new BitString(64), new List<BitString>()));
            _modeFactory = new Mock<IModeBlockCipherFactory>();
            _modeFactory
                .Setup(s => s.GetCounterCipher(
                    It.IsAny<IBlockCipherEngine>(),
                    It.IsAny<ICounter>())
                )
                .Returns(_mode.Object);
            _counter = new Mock<ICounter>();
            _counterFactory = new Mock<ICounterFactory>();
            _counterFactory.Setup(s =>
                    s.GetCounter(
                        It.IsAny<IBlockCipherEngine>(),
                        It.IsAny<CounterTypes>(),
                        It.IsAny<BitString>()
                    ))
                .Returns(_counter.Object);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GenerateShouldReturnTestCaseGenerateResponse(bool isSample)
        {
            _subject = new TestCaseGeneratorCounterDecrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(true, true), isSample);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFail()
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCounterResult("Fail"));

            _subject = new TestCaseGeneratorCounterDecrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(true, true), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnException()
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception());

            _subject = new TestCaseGeneratorCounterDecrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(true, true), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeOperationWhenIsSampleIsTrue()
        {
            _subject = new TestCaseGeneratorCounterDecrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            _subject.Generate(GetTestGroup(true, true), true);

            _mode.Verify(v => v.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()),
                Times.AtLeastOnce,
                nameof(_mode.Object.ProcessPayload)
            );
        }

        [Test]
        public void GenerateShouldNotInvokeOperationWhenIsSampleIsFalse()
        {
            _subject = new TestCaseGeneratorCounterDecrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            _subject.Generate(GetTestGroup(true, true), false);

            _mode.Verify(v => v.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()),
                Times.Never,
                nameof(_mode.Object.ProcessPayload)
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var fakeIvs = new List<BitString>();

            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCounterResult(fakeCipher, fakeIvs));

            _subject = new TestCaseGeneratorCounterDecrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(true, true), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty((result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty((result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty((result.TestCase).IVs.ToString(), "Ivs");
            Assert.IsNotEmpty((result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
        }

        private TestGroup GetTestGroup(bool increment, bool overflow)
        {
            return new TestGroup
            {
                KeyLength = 128,
                IncrementalCounter = increment,
                OverflowCounter = overflow
            };
        }
    }
}
