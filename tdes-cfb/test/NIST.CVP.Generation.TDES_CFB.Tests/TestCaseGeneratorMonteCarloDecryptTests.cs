using Moq;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;
using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloDecryptTests
    {

        private Mock<IRandom800_90> _mockRandom;
        private Mock<ICFBModeMCT> _mockMCT;
        private TestCaseGeneratorMonteCarloDecrypt _subject;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_CFB", true);
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns((int i) => new BitString(i));
            _mockMCT = new Mock<ICFBModeMCT>();
            _mockMCT.SetupGet(s => s.Algo).Returns(AlgoMode.TDES_CFB1);
            _subject = new TestCaseGeneratorMonteCarloDecrypt(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallAlgoEncryptFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 3
            };
            TestCase testCase = new TestCase();
            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 3
            };
            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockMCT.Setup(s => s.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 3
            };
            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }



        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
