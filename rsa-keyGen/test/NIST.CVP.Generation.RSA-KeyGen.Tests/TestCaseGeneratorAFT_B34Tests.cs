using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Moq;
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
    public class TestCaseGeneratorAFT_B34Tests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GenerateShouldReturnTestCaseGenerateResponse(bool infoGeneratedByServer)
        {
            var subject = new TestCaseGeneratorAFT_B34(GetRandomMock().Object, GetPrimeGenMock().Object);

            var result = subject.Generate(GetTestGroup(infoGeneratedByServer), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullTestCaseOnFailedKeyGen()
        {
            var keyGen = GetPrimeGenMock();
            keyGen
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult("Fail"));

            var subject = new TestCaseGeneratorAFT_B34(GetRandomMock().Object, keyGen.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullTestCaseOnExceptionKeyGen()
        {
            var keyGen = GetPrimeGenMock();
            keyGen
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorAFT_B34(GetRandomMock().Object, keyGen.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeKeyGenOperation()
        {
            var keyGen = GetPrimeGenMock();
            keyGen
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(0, 0));

            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            rand
                .Setup(s => s.GetRandomBigInteger(It.IsAny<BigInteger>(), It.IsAny<BigInteger>()))
                .Returns(BigInteger.One);

            var subject = new TestCaseGeneratorAFT_B34(rand.Object, keyGen.Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assume.That(result.Success);
            keyGen.Verify(v => v.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()), Times.AtLeastOnce, "GeneratePrimes should be invoked");
        }

        [Test]
        public void GenerateShouldNotInvokeKeyGenOperationWhenClientGeneratesContent()
        {
            var keyGen = GetPrimeGenMock();
            keyGen
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(0, 0));

            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            rand
                .Setup(s => s.GetRandomBigInteger(It.IsAny<BigInteger>(), It.IsAny<BigInteger>()))
                .Returns(BigInteger.One);

            var subject = new TestCaseGeneratorAFT_B34(rand.Object, keyGen.Object);

            var result = subject.Generate(GetTestGroup(false), false);

            Assume.That(result.Success);
            Assume.That(result.TestCase.Deferred);
            keyGen.Verify(v => v.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()), Times.Never, "GeneratePrimes should not be invoked");
        }

        [Test]
        public void ResultShouldNotHaveContentWhenServerGeneratesInfo()
        {
            var subject = new TestCaseGeneratorAFT_B34(new Random800_90(), new AllProvablePrimesWithConditionsGenerator());

            var group = GetTestGroup(false);
            group.PubExp = PubExpModes.FIXED;
            group.FixedPubExp = new BitString("ABCD");

            var result = subject.Generate(group, false);

            Assume.That(result.Success);

            var testCase = (TestCase) result.TestCase;
            Assert.AreEqual(testCase.Key.PubKey.N, BigInteger.Zero);
            Assert.AreEqual(testCase.Key.PrivKey.P, BigInteger.Zero);
            Assert.AreEqual(testCase.Key.PrivKey.Q, BigInteger.Zero);
        }

        [Test]
        [Ignore("Longer test to make sure things work quickly")]
        public void GenerateShouldActuallyGenerateATestCaseWithValidData()
        {
            var subject = new TestCaseGeneratorAFT_B34(new Random800_90(), new AllProvablePrimesWithConditionsGenerator());
            var result = subject.Generate(GetTestGroup(), true);

            var testCase = (TestCase)result.TestCase;

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreNotEqual(testCase.Seed, 0);
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

        private Mock<AllProvablePrimesWithConditionsGenerator> GetPrimeGenMock()
        {
            return new Mock<AllProvablePrimesWithConditionsGenerator>();
        }

        private TestGroup GetTestGroup(bool info = true)
        {
            return new TestGroup
            {
                InfoGeneratedByServer = info,
                Modulo = 2048,
                PubExp = PubExpModes.RANDOM,
                HashAlg = new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224}
            };
        }
    }
}
