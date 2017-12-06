using System;
using Moq;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Math;
using NLog;
using NUnit.Framework;
using NIST.CVP.Tests.Core;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloEncryptTests
    {

        private Mock<IRandom800_90> _mockRandom;
        private Mock<ICFBPModeMCT> _mockMCT;
        private TestCaseGeneratorMonteCarloEncrypt _subject;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_OFB", true);
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(new BitString(1));
            _mockMCT = new Mock<ICFBPModeMCT>();
            _mockMCT.SetupProperty(x => x.Algo, Algo.TDES_CFBP1);
            _subject = new TestCaseGeneratorMonteCarloEncrypt(_mockRandom.Object, _mockMCT.Object);
        }

        //[Test]
        //[TestCase(1)]
        //[TestCase(2)]
        //public void ShouldCallRandomTwiceOnceForKeyOnceForCipherText(int keyingOption)
        //{
        //    TestGroup testGroup = new TestGroup()
        //    {
        //        KeyingOption = keyingOption
        //    };
        //    _subject.Generate(testGroup, false);

        //    _mockRandom.Verify(v => v.GetRandomBitString(64 * keyingOption), nameof(keyingOption));
        //    _mockRandom.Verify(v => v.GetRandomBitString(64));
        //}

        [Test]
        public void ShouldCallAlgoEncryptFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
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

            _mockMCT.Verify(v => v.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new MCTResult<AlgoArrayResponseCfbp>(errorMessage));

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
            _mockMCT.Setup(s => s.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
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
