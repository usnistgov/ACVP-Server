using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _mockTestCaseGeneratorFactory;
        private Mock<IDeferredTestCaseGenerator<TestGroup, TestCase>> _mockDeferredTestCaseGenerator;
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _mockTestCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _mockDeferredTestCaseGenerator = new Mock<IDeferredTestCaseGenerator<TestGroup, TestCase>>();

            _mockDeferredTestCaseGenerator
                .Setup(s => s.CompleteDeferredTestCase(It.IsAny<TestGroup>(), It.IsAny<TestCase>()))
                .Returns(new TestCaseGenerateResponse(new TestCase()));

            _mockDeferredTestCaseGenerator
                .Setup(s => s.RecombineTestCases(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new TestCaseGenerateResponse(new TestCase()));

            _mockTestCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(_mockDeferredTestCaseGenerator.Object);

            _subject = new TestCaseValidatorFactory(_mockTestCaseGeneratorFactory.Object);
        }

        [Test]
        [TestCase("aft", true,   typeof(TestCaseValidatorAFT))]
        [TestCase("aft", false,  typeof(TestCaseValidatorAFT))]
        [TestCase("gdt", true,   typeof(TestCaseValidatorGDT))]
        [TestCase("gdt", false,  typeof(TestCaseValidatorGDT))]
        [TestCase("kat", true,   typeof(TestCaseValidatorKAT))]
        [TestCase("kat", false,  typeof(TestCaseValidatorKAT))]
        [TestCase("junk", true,  typeof(TestCaseValidatorNull))]
        [TestCase("junk", false, typeof(TestCaseValidatorNull))]
        public void ShouldReturnCorrectValidatorTypeDependentOnFunction(string testType, bool isDeferred,
            Type expectedType)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, testType, isDeferred);

            var results = _subject.GetValidators(testVectorSet, suppliedResults);

            Assert.AreEqual(1, results.Count(), "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        [Test]
        [TestCase("aft", true, 1)]
        [TestCase("aft", false, 0)]
        [TestCase("gdt", true, 0)]
        [TestCase("gdt", false, 0)]
        [TestCase("kat", true, 0)]
        [TestCase("kat", false, 0)]
        [TestCase("junk", true, 0)]
        [TestCase("junk", false, 0)]
        public void ShouldOnlyCallTestCaseGeneratorOnDeferredGroups(string testType, bool isDeferred, int timesToCall)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, testType, isDeferred);

            var results = _subject.GetValidators(testVectorSet, suppliedResults);

            Assert.AreEqual(1, results.Count(), "Expected 1 validator");
            _mockDeferredTestCaseGenerator.Verify(
                v => v.RecombineTestCases(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()),
                Times.Exactly(timesToCall), nameof(_mockDeferredTestCaseGenerator.Object.RecombineTestCases));
            _mockDeferredTestCaseGenerator.Verify(
                v => v.CompleteDeferredTestCase(It.IsAny<TestGroup>(), It.IsAny<TestCase>()),
                Times.Exactly(timesToCall), nameof(_mockDeferredTestCaseGenerator.Object.CompleteDeferredTestCase));
        }

        private void GetData(ref TestVectorSet testVectorSet, ref List<TestCase> suppliedResults, string testType,
            bool isDeferred)
        {
            var eHex = "BEEFFACE";

            testVectorSet = new TestVectorSet
            {
                Algorithm = "",
                TestGroups = new List<ITestGroup>
                {
                    new TestGroup
                    {
                        TestType = testType,
                        InfoGeneratedByServer = isDeferred,
                        Modulo = 2048,
                        PubExp = PubExpModes.FIXED,
                        FixedPubExp = new BitString(eHex),
                        Tests = new List<ITestCase>
                        {
                            new TestCase
                            {
                                TestCaseId = 1,
                                Deferred = isDeferred,
                                Key = new KeyPair {PubKey = new PublicKey {E = new BitString(eHex).ToPositiveBigInteger() }}
                            }
                        }
                    }
                }
            };

            suppliedResults = new List<TestCase>
            {
                new TestCase
                {
                    TestCaseId = 1,
                    Key = new KeyPair
                    {
                        PrivKey = new PrivateKey
                        {
                            D = 1,
                            DMP1 = 1,
                            DMQ1 = 1,
                            IQMP = 1,
                            P = 1,
                            Q = 1
                        },
                        PubKey = new PublicKey
                        {
                            E = new BitString(eHex).ToPositiveBigInteger(),
                            N = 1
                        }
                    },
                    Seed = new BitString("BEEF"),
                    Bitlens = new [] {1, 2, 3, 4}
                }
            };
        }
    }
}
