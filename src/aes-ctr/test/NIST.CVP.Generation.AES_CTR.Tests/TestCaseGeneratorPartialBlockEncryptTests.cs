using System;
using System.Collections.Generic;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorPartialBlockEncryptTests
    {
        private TestCaseGeneratorPartialBlockEncrypt _subject;

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
                .Returns(() => new SymmetricCounterResult(new BitString(128), new List<BitString>()));
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
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            _subject = new TestCaseGeneratorPartialBlockEncrypt(_random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFail()
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCounterResult("Fail"));

            _subject = new TestCaseGeneratorPartialBlockEncrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnException()
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception());

            _subject = new TestCaseGeneratorPartialBlockEncrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeOperation()
        {
            _subject = new TestCaseGeneratorPartialBlockEncrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            _subject.Generate(GetTestGroup(), true);

            _mode.Verify(v => v.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()),
                Times.AtLeastOnce,
                nameof(_mode.Object.ProcessPayload)
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });

            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCounterResult(fakeCipher, null));

            _subject = new TestCaseGeneratorPartialBlockEncrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object,
                _counterFactory.Object
            );

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty((result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty((result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty((result.TestCase).IV.ToString(), "Iv");
            Assert.IsNotEmpty((result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                KeyLength = 128,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 64))
            };
        }
    }
}
