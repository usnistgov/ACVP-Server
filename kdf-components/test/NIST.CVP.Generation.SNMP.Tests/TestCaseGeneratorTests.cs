using System;
using Moq;
using NIST.CVP.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.Crypto.SNMP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SNMP.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetSnmpMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedSnmp()
        {
            var snmp = GetSnmpMock();
            snmp
                .Setup(s => s.KeyLocalizationFunction(It.IsAny<BitString>(), It.IsAny<string>()))
                .Returns(new SnmpResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, snmp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionSnmp()
        {
            var snmp = GetSnmpMock();
            snmp
                .Setup(s => s.KeyLocalizationFunction(It.IsAny<BitString>(), It.IsAny<string>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, snmp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateSnmpOperation()
        {
            var snmp = GetSnmpMock();

            var subject = new TestCaseGenerator(GetRandomMock().Object, snmp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            snmp.Verify(s => s.KeyLocalizationFunction(It.IsAny<BitString>(), It.IsAny<string>()),
                Times.AtLeastOnce,
                "KeyLocalizationFunction should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });

            var snmp = GetSnmpMock();
            snmp
                .Setup(s => s.KeyLocalizationFunction(It.IsAny<BitString>(), It.IsAny<string>()))
                .Returns(new SnmpResult(fakeKey));

            var subject = new TestCaseGenerator(GetRandomMock().Object, snmp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SharedKey.ToString(), "SharedKey");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            random
                .Setup(s => s.GetRandomAlphaCharacters(It.IsAny<int>()))
                .Returns("abcdefghijklmnop");
            return random;
        }

        private Mock<ISnmp> GetSnmpMock()
        {
            return new Mock<ISnmp>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                EngineId = new BitString("abcdabcdabcdabcd"),
                PasswordLength = 16
            };
        }
    }
}
