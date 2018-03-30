using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.EccComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        private TestCaseGenerator _subject;

        private Mock<IEccCurve> _curve;
        private Mock<IDsaEcc> _dsa;
        private Mock<IEccDhComponent> _eccDhComponent;

        [SetUp]
        public void Setup()
        {
            _curve = new Mock<IEccCurve>();
            _dsa = new Mock<IDsaEcc>();
            _eccDhComponent = new Mock<IEccDhComponent>();

            _dsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(() => new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 1), 1)));
            _eccDhComponent
                .Setup(s => s.GenerateSharedSecret(
                    It.IsAny<EccDomainParameters>(), 
                    It.IsAny<EccKeyPair>(),
                    It.IsAny<EccKeyPair>()
                ))
                .Returns(new SharedSecretResponse(new BitString(1)));

            _subject = new TestCaseGenerator(_curve.Object, _dsa.Object, _eccDhComponent.Object);
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ShouldGenerateSuccessfully(bool isSample)
        {
            var result = _subject.Generate(GetTestGroup(), isSample);

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        public void ShouldInvokeKeyGenCorrectNumberOfTimes(bool isSample, int numberOfInvokes)
        {
            _subject.Generate(GetTestGroup(), isSample);

            _dsa.Verify(
                v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), 
                Times.Exactly(numberOfInvokes), 
                nameof(_dsa.Object.GenerateKeyPair)
            );
        }

        [Test]
        public void ShouldPopulateCorrectPropertiesNotSample()
        {
            var result = _subject.Generate(GetTestGroup(), false);

            Assume.That(result.Success, "success");
            var testCase = (TestCase)result.TestCase;

            Assert.IsTrue(testCase.KeyPairPartyServer.PrivateD != 0, nameof(testCase.KeyPairPartyServer));
            Assert.IsTrue(testCase.KeyPairPartyIut.PrivateD == 0, nameof(testCase.KeyPairPartyIut));
            Assert.IsTrue(testCase.Deferred, nameof(testCase.Deferred));
            Assert.IsNull(testCase.Z, nameof(testCase.Z));
        }

        public void ShouldPopulateCorrectPropertiesSample()
        {
            var result = _subject.Generate(GetTestGroup(), true);

            Assume.That(result.Success, "success");
            var testCase = (TestCase)result.TestCase;

            Assert.IsTrue(testCase.KeyPairPartyServer.PrivateD == 0, nameof(testCase.KeyPairPartyServer));
            Assert.IsTrue(testCase.KeyPairPartyIut.PrivateD == 0, nameof(testCase.KeyPairPartyIut));
            Assert.IsFalse(testCase.Deferred, nameof(testCase.Deferred));
            Assert.IsNotNull(testCase.Z, nameof(testCase.Z));
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}