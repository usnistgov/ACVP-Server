using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests
{
    [TestFixture, FastCryptoTest]
    public class KasTestsEcc
    {
        private Kas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _subject;
        private Mock<IScheme<SchemeParametersBase<KasDsaAlgoAttributesEcc>, KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair>> _scheme;

        [SetUp]
        public void Setup()
        {
            _scheme = new Mock<IScheme<SchemeParametersBase<KasDsaAlgoAttributesEcc>, KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair>>();
            _scheme.Setup(s => s.SetDomainParameters(It.IsAny<EccDomainParameters>()));
            _scheme
                .Setup(s => s.ReturnPublicInfoThisParty())
                .Returns(It.IsAny<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>>());
            _scheme
                .Setup(s => s.ComputeResult(It.IsAny<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>>()))
                .Returns(new KasResult(string.Empty));

            _subject = new Kas<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair>(_scheme.Object);
        }

        [Test]
        public void ShouldInvokeSchemeSetDomainParameters()
        {
            _subject.SetDomainParameters(It.IsAny<EccDomainParameters>());
            _scheme.Verify(v => v.SetDomainParameters(It.IsAny<EccDomainParameters>()),
                Times.Once,
                nameof(_scheme.Object.SetDomainParameters)
            );
        }

        [Test]
        public void ShouldInvokeSchemeReturnPublicInfoThisParty()
        {
            _subject.ReturnPublicInfoThisParty();
            _scheme.Verify(v => v.ReturnPublicInfoThisParty(),
                Times.Once,
                nameof(_scheme.Object.ReturnPublicInfoThisParty)
            );
        }

        [Test]
        public void ShouldInvokeSchemeComputeResult()
        {
            _subject.ComputeResult(It.IsAny<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>>());
            _scheme.Verify(v => v.ComputeResult(It.IsAny<OtherPartySharedInformation<EccDomainParameters, EccKeyPair>>()),
                Times.Once,
                nameof(_scheme.Object.ComputeResult)
            );
        }
    }
}
