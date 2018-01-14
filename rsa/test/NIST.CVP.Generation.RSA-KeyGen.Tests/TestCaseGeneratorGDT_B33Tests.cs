using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorGDT_B33Tests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorGDT_B33(GetRandomMock().Object, GetPrimeGenMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GeneratedTestCaseShouldContainEValueWhenFixed()
        {
            var subject = new TestCaseGeneratorGDT_B33(GetRandomMock().Object, GetPrimeGenMock().Object);

            var group = new TestGroup
            {
                Modulo = 2048,
                FixedPubExp = new BitString("1234"),
                PubExp = PubExpModes.FIXED
            };

            var result = subject.Generate(group, false);

            Assume.That(result.Success);

            var testCase = (TestCase) result.TestCase;

            Assert.IsNotNull(testCase, $"{nameof(testCase)} should not be null");
            Assert.AreEqual(new BitString("1234").ToPositiveBigInteger(), testCase.Key.PubKey.E);
        }

        [Test]
        public void GeneratedTestCaseShouldNotContainEValueWhenRandom()
        {
            var subject = new TestCaseGeneratorGDT_B33(GetRandomMock().Object, GetPrimeGenMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assume.That(result.Success);

            var testCase = (TestCase)result.TestCase;

            Assert.IsNotNull(testCase, $"{nameof(testCase)} should not be null");
            Assert.IsNull(testCase.Key, $"{nameof(testCase.Key)} should be null");
        }

        [Test]
        [Ignore("Longer test to make sure things work quickly")]
        public void GenerateShouldActuallyGenerateATestCaseWithValidData()
        {
            var subject = new TestCaseGeneratorGDT_B33(new Random800_90(), new RandomProbablePrimeGenerator());
            var result = subject.Generate(GetTestGroup(), true);

            var testCase = (TestCase)result.TestCase;

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreNotEqual(testCase.Key.PubKey.E, 0);
            Assert.AreNotEqual(testCase.Key.PubKey.N, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.P, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.Q, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.D, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.DMP1, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.DMQ1, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.IQMP, 0);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<RandomProbablePrimeGenerator> GetPrimeGenMock()
        {
            return new Mock<RandomProbablePrimeGenerator>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Modulo = 2048,
                PubExp = PubExpModes.RANDOM
            };
        }
    }
}
