using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloEncryptTests
    {

        private Mock<IRandom800_90> _mockRandom;
        private Mock<ITDES_CBC_MCT> _mockMCT;
        private TestCaseGeneratorMonteCarloEncrypt _subject;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_CBC", true);
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(new BitString(1));
            _mockMCT = new Mock<ITDES_CBC_MCT>();
            _subject = new TestCaseGeneratorMonteCarloEncrypt(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldCallRandomTwiceOnceForKeyOnceForCipherText(int numberOfKeys)
        {
            TestGroup testGroup = new TestGroup()
            {
                NumberOfKeys = numberOfKeys
            };
            _subject.Generate(testGroup, false);

            _mockRandom.Verify(v => v.GetRandomBitString(64 * numberOfKeys), nameof(numberOfKeys));
            _mockRandom.Verify(v => v.GetRandomBitString(64));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                NumberOfKeys = 3
            };
            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                NumberOfKeys = 3
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
                .Returns(new MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                NumberOfKeys = 3
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
                NumberOfKeys = 3
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
