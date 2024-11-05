using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.EccComponent
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetKasEccComponentTestAsync(It.IsAny<KasEccComponentParameters>()))
                .Returns(() => Task.FromResult(new KasEccComponentResult
                {
                    PrivateKeyIut = 0,
                    PrivateKeyServer = 1,
                    PublicKeyIutX = 2,
                    PublicKeyIutY = 3,
                    PublicKeyServerX = 4,
                    PublicKeyServerY = 5,
                    Z = new BitString("00")
                }));

            _subject = new TestCaseGenerator(_oracle.Object);
        }

        private TestCaseGenerator _subject;

        private Mock<IOracle> _oracle;

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGenerateSuccessfully(bool isSample)
        {
            var result = await _subject.GenerateAsync(GetTestGroup(), isSample);

            Assert.That(result.Success, Is.True);
        }

        //[Test]
        //[TestCase(false, 1)]
        //[TestCase(true, 2)]
        //public void ShouldInvokeKeyGenCorrectNumberOfTimes(bool isSample, int numberOfInvokes)
        //{
        //    _subject.Generate(GetTestGroup(), isSample);

        //    _dsa.Verify(
        //        v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), 
        //        Times.Exactly(numberOfInvokes), 
        //        nameof(_dsa.Object.GenerateKeyPair)
        //    );
        //}

        //[Test]
        //public void ShouldPopulateCorrectPropertiesNotSample()
        //{
        //    var result = _subject.Generate(GetTestGroup(), false);

        //    Assert.That(result.Success, "success");
        //    var testCase = (TestCase)result.TestCase;

        //    Assert.IsTrue(testCase.KeyPairPartyServer.PrivateD != 0, nameof(testCase.KeyPairPartyServer));
        //    Assert.IsTrue(testCase.KeyPairPartyIut.PrivateD == 0, nameof(testCase.KeyPairPartyIut));
        //    Assert.IsTrue(testCase.Deferred, nameof(testCase.Deferred));
        //    Assert.IsNull(testCase.Z, nameof(testCase.Z));
        //}

        //public void ShouldPopulateCorrectPropertiesSample()
        //{
        //    var result = _subject.Generate(GetTestGroup(), true);

        //    Assert.That(result.Success, "success");
        //    var testCase = (TestCase)result.TestCase;

        //    Assert.IsTrue(testCase.KeyPairPartyServer.PrivateD == 0, nameof(testCase.KeyPairPartyServer));
        //    Assert.IsTrue(testCase.KeyPairPartyIut.PrivateD == 0, nameof(testCase.KeyPairPartyIut));
        //    Assert.IsFalse(testCase.Deferred, nameof(testCase.Deferred));
        //    Assert.IsNotNull(testCase.Z, nameof(testCase.Z));
        //}

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
