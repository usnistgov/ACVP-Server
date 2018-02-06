using System.Numerics;
using Moq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAftTests
    {
        private readonly IKeyComposerFactory _keyComposerFactory = new KeyComposerFactory();
        private readonly IShaFactory _shaFactory = new ShaFactory();

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GenerateShouldReturnTestCaseGenerateResponse(bool infoGeneratedByServer)
        {
            var keyBuilder = GetKeyBuilderMock();
            keyBuilder
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair()));

            var subject = new TestCaseGeneratorAft(GetRandomMock().Object, keyBuilder.Object, _keyComposerFactory, _shaFactory);

            var result = subject.Generate(GetRandomETestGroup(infoGeneratedByServer), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldInvokeKeyGenOperation()
        {
            var keyBuilder = GetKeyBuilderMock();
            keyBuilder
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair()));

            var subject = new TestCaseGeneratorAft(GetRandomMock().Object, keyBuilder.Object, _keyComposerFactory, _shaFactory);

            var testGroup = GetFixedETestGroup();
            testGroup.InfoGeneratedByServer = true;

            var result = subject.Generate(testGroup, false);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            keyBuilder.Verify(v => v.Build(), Times.Once, "KeyBuilder.Build() should be invoked");
        }

        [Test]
        public void GenerateShouldNotInvokeKeyGenOperationWhenClientGeneratesContent()
        {
            var keyBuilder = GetKeyBuilderMock();
            keyBuilder
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair()));

            var subject = new TestCaseGeneratorAft(GetRandomMock().Object, keyBuilder.Object, _keyComposerFactory, _shaFactory);

            var result = subject.Generate(GetRandomETestGroup(false), false);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.IsTrue(result.TestCase.Deferred, result.ErrorMessage);
            keyBuilder.Verify(v => v.Build(), Times.Never, "KeyBuilder.Build() should not be invoked");
        }

        [Test]
        public void ResultShouldNotHaveContentWhenServerGeneratesInfo()
        {
            var subject = new TestCaseGeneratorAft(GetRandomMock().Object, GetKeyBuilderMock().Object, _keyComposerFactory, _shaFactory);

            var group = GetFixedETestGroup();

            var result = subject.Generate(group, false);

            Assert.IsTrue(result.Success, result.ErrorMessage);

            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(testCase.Key.PubKey.N, BigInteger.Zero);
            Assert.IsNull(testCase.Key.PrivKey);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        [Ignore("Longer test to make sure things work quickly")]
        public void GenerateShouldActuallyGenerateATestCaseWithValidData(bool crtForm)
        {
            var keyBuilder = new KeyBuilder(new PrimeGeneratorFactory());
            var testGroup = GetRandomETestGroup(false);

            testGroup.KeyFormat = crtForm ? PrivateKeyModes.Crt : PrivateKeyModes.Standard;

            var subject = new TestCaseGeneratorAft(new Random800_90(), keyBuilder, _keyComposerFactory, _shaFactory);
            var result = subject.Generate(GetRandomETestGroup(false), true);

            var testCase = (TestCase)result.TestCase;

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreNotEqual(testCase.Seed, 0);
            Assert.AreNotEqual(testCase.Key.PubKey.E, 0);
            Assert.AreNotEqual(testCase.Key.PubKey.N, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.P, 0);
            Assert.AreNotEqual(testCase.Key.PrivKey.Q, 0);

            if (crtForm)
            {
                Assert.AreNotEqual(((CrtPrivateKey)testCase.Key.PrivKey).DMP1, 0);
                Assert.AreNotEqual(((CrtPrivateKey)testCase.Key.PrivKey).DMQ1, 0);
                Assert.AreNotEqual(((CrtPrivateKey)testCase.Key.PrivKey).IQMP, 0);
            }
            else
            {
                Assert.AreNotEqual(((PrivateKey)testCase.Key.PrivKey).D, 0);
            }

            Assert.AreNotEqual(testCase.XP, 0);
            Assert.AreNotEqual(testCase.XQ, 0);
            Assert.AreNotEqual(testCase.XP1, 0);
            Assert.AreNotEqual(testCase.XP2, 0);
            Assert.AreNotEqual(testCase.XQ1, 0);
            Assert.AreNotEqual(testCase.XQ2, 0);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCDEFABCDEF"));    // Needs to be between 32-64 bits

            randMock
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            return randMock;
        }

        private Mock<IKeyBuilder> GetKeyBuilderMock()
        {
            var mock = new Mock<IKeyBuilder>();
            mock.SetReturnsDefault(mock.Object);
            return mock;
        }

        private TestGroup GetRandomETestGroup(bool info = true)
        {
            return new TestGroup
            {
                InfoGeneratedByServer = info,
                Modulo = 2048,
                PubExp = PublicExponentModes.Random,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224)
            };
        }

        private TestGroup GetFixedETestGroup()
        {
            return new TestGroup
            {
                Modulo = 2048,
                PubExp = PublicExponentModes.Fixed,
                FixedPubExp = new BitString("ACED"),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                KeyFormat = PrivateKeyModes.Standard
            };
        }

        private TestCase GetTestCase()
        {
            var rand = new Random800_90();
            var bitlens = new[] { 144, 180, 200, 204 };

            return new TestCase
            {
                TestCaseId = 1,
                Deferred = true,
                Key = new KeyPair { PubKey = new PublicKey { E = 5 } },
                Seed = new BitString("BEEFFACE"),
                Bitlens = bitlens,
                XP = rand.GetRandomBitString(bitlens[0]),
                XQ = rand.GetRandomBitString(bitlens[0]),
                XP1 = rand.GetRandomBitString(bitlens[0]),
                XP2 = rand.GetRandomBitString(bitlens[1]),
                XQ1 = rand.GetRandomBitString(bitlens[2]),
                XQ2 = rand.GetRandomBitString(bitlens[3]),
            };
        }
    }
}

