using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;
using NIST.CVP.Generation.DSA.ECC.KeyVer.TestCaseExpectations;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorGTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBigInteger(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var eccMock = GetEccMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            var subject = new TestCaseGenerator(rand.Object, eccMock.Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("AB"));

            var eccMock = GetEccMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            var subject = new TestCaseGenerator(rand.Object, eccMock.Object);
            var result = subject.Generate(GetTestGroup(), false);

            eccMock.Verify(v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), Times.Once, "Call GenerateKeyPair once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;

            // These values could be modified during the generate process
            //Assert.AreEqual(BigInteger.One, testCase.KeyPair.PublicQ.X);
            //Assert.AreEqual(BigInteger.One * 2, testCase.KeyPair.PublicQ.Y);

            Assert.AreEqual(BigInteger.One * 3, testCase.KeyPair.PrivateD);
        }

        [Test]
        public void GenerateShouldGenerateOneOfEachFailureReason()
        {
            var group = GetTestGroup();

            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var eccMock = GetEccMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            var subject = new TestCaseGenerator(rand.Object, eccMock.Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(group, false);

                Assert.IsTrue(result.Success);
                group.Tests.Add(result.TestCase);
            }

            var failCases = 0;
            var passCases = 0;
            foreach (var testCase in group.Tests.Select(s => (TestCase)s))
            {
                if (testCase.FailureTest)
                {
                    failCases++;
                }
                else
                {
                    passCases++;
                }
            }

            Assert.AreEqual(8, failCases);
            Assert.AreEqual(4, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaEcc> GetEccMock()
        {
            return new Mock<IDsaEcc>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                DomainParameters = new EccDomainParameters(new PrimeCurve(Curve.P192, 0, 0, new EccPoint(0, 0), 0)),
                TestCaseExpectationProvider = new TestCaseExpectationProvider()
            };
        }
    }
}
