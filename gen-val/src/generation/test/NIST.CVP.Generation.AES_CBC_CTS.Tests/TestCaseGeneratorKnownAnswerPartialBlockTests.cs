using Moq;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_CBC_CTS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorKnownAnswerTests
    {
        private TestCaseGeneratorKnownAnswerPartialBlock _subject;

        private readonly Mock<IOracle> _oracle = new Mock<IOracle>();

        [SetUp]
        public void Setup()
        {
            _oracle
                .Setup(s => s.GetAesCaseAsync(It.IsAny<AesWithPayloadParameters>()))
                .Returns(() => Task.FromResult(new AesResult()));
        }

        [TestCase(128, "gfsbox")]
        [TestCase(192, "gfsbox")]
        [TestCase(256, "gfsbox")]
        [TestCase(128, "keysbox")]
        [TestCase(192, "keysbox")]
        [TestCase(256, "keysbox")]
        [TestCase(128, "vartxt")]
        [TestCase(192, "vartxt")]
        [TestCase(256, "vartxt")]
        [TestCase(128, "varkey")]
        [TestCase(192, "varkey")]
        [TestCase(256, "varkey")]
        public async Task ShouldReturnResponseWithCollectionMatchingKeySize(int keyLength, string katType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = AlgoMode.AES_CBC_CS3_v1_0,
                KeyLength = keyLength,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 1))
            };

            _subject = new TestCaseGeneratorKnownAnswerPartialBlock(_oracle.Object, keyLength, katType);
            var result = await _subject.GenerateAsync(testGroup, false);

            Assert.IsTrue(result.Success, nameof(result.Success));
            Assert.AreEqual(keyLength, (result.TestCase).Key.BitLength, nameof(keyLength));
        }

        [Test]
        [TestCase(1, "gfsbox")]
        [TestCase(2, "keysbox")]
        [TestCase(3, "vartxt")]
        [TestCase(4, "varkey")]
        public void ShouldThrowOnInvalidKeySize(int keyLength, string katType)
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => _subject = new TestCaseGeneratorKnownAnswerPartialBlock(_oracle.Object, keyLength, katType)
            );
        }

        [Test]
        [TestCase(128, "gfsbox")]
        [TestCase(192, "gfsbox")]
        [TestCase(256, "gfsbox")]
        [TestCase(128, "keysbox")]
        [TestCase(192, "keysbox")]
        [TestCase(256, "keysbox")]
        [TestCase(128, "vartxt")]
        [TestCase(192, "vartxt")]
        [TestCase(256, "vartxt")]
        [TestCase(128, "varkey")]
        [TestCase(192, "varkey")]
        [TestCase(256, "varkey")]
        public async Task ShouldReturnSuccessResponseWhenKatsGenerated(int keyLength, string katType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = AlgoMode.AES_CBC_CS3_v1_0,
                KeyLength = keyLength,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 1))
            };

            _subject = new TestCaseGeneratorKnownAnswerPartialBlock(_oracle.Object, keyLength, katType);

            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i < _subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await _subject.GenerateAsync(testGroup, false));
            }

            Assert.IsTrue(results.TrueForAll(tfa => tfa.Success));
        }

        [Test]
        [TestCase(128, "gfsbox")]
        [TestCase(192, "gfsbox")]
        [TestCase(256, "gfsbox")]
        [TestCase(128, "keysbox")]
        [TestCase(192, "keysbox")]
        [TestCase(256, "keysbox")]
        [TestCase(128, "vartxt")]
        [TestCase(192, "vartxt")]
        [TestCase(256, "vartxt")]
        [TestCase(128, "varkey")]
        [TestCase(192, "varkey")]
        [TestCase(256, "varkey")]
        public async Task ShouldReturnFailureResponseWhenKatsExhausted(int keyLength, string katType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = AlgoMode.AES_CBC_CS3_v1_0,
                KeyLength = keyLength,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 1))
            };

            _subject = new TestCaseGeneratorKnownAnswerPartialBlock(_oracle.Object, keyLength, katType);

            var results = new List<TestCaseGenerateResponse<TestGroup, TestCase>>();
            for (int i = 0; i <= _subject.NumberOfTestCasesToGenerate; i++)
            {
                results.Add(await _subject.GenerateAsync(testGroup, false));
            }

            Assert.IsTrue(results.Any(a => !a.Success));
        }
    }
}