using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMCTEncryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>> _mockMct;
        private Mock<IMonteCarloFactoryAes> _mockMctFactory;
        private TestCaseGeneratorMCTEncrypt _subject;

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(() => new BitString(128));
            _mockMct = new Mock<IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>();
            _mockMctFactory = new Mock<IMonteCarloFactoryAes>();
            _mockMctFactory
                .Setup(s => s.GetInstance(
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mockMct.Object);
        }

        [Test]
        public void ShouldCallRandomTwiceOnceForKeyOnceFor()
        {
            int keyLength = 256;
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                KeyLength = keyLength
            };
            _subject = new TestCaseGeneratorMCTEncrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            _subject.Generate(testGroup, false);

            _mockRandom.Verify(v => v.GetRandomBitString(keyLength), nameof(keyLength));
            _mockRandom.Verify(v => v.GetRandomBitString(128));
        }

        [Test]
        public void ShouldCallAlgoFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                KeyLength = 128
            };
            _subject = new TestCaseGeneratorMCTEncrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            _subject.Generate(testGroup, false);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldCallAlgoFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                KeyLength = 128
            };
            TestCase testCase = new TestCase()
            {
                IV = new BitString(128),
                Key = new BitString(128),
                PlainText = new BitString(128),
                CipherText = new BitString(128)
            };
            _subject = new TestCaseGeneratorMCTEncrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            _subject.Generate(testGroup, testCase);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMct
                .Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                KeyLength = 128
            };
            _subject = new TestCaseGeneratorMCTEncrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            var result = _subject.Generate(testGroup, false);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockMct
                .Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                KeyLength = 128
            };
            _subject = new TestCaseGeneratorMCTEncrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            var result = _subject.Generate(testGroup, false);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}
