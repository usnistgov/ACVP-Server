﻿using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMCTDecryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>> _mockMCT;
        private TestCaseGeneratorMCTDecrypt _subject;

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockMCT = new Mock<IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>();
            _subject = new TestCaseGeneratorMCTDecrypt(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallRandomTwiceOnceForKeyOnceForCipherText()
        {
            int keyLength = 256;
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };
            _subject.Generate(testGroup, false);

            _mockRandom.Verify(v => v.GetRandomBitString(keyLength), nameof(keyLength));
            _mockRandom.Verify(v => v.GetRandomBitString(128));
        }

        [Test]
        public void ShouldCallAlgoDecryptFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldCallAlgoDecryptFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            TestCase testCase = new TestCase();
            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}