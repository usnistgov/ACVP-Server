using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests
{
    [TestFixture,  FastCryptoTest]
    public class KasTestsFfc
    {
        private Kas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _subject;
        private Mock<IScheme<SchemeParametersBase<KasDsaAlgoAttributesFfc>, KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>> _scheme;

        [SetUp]
        public void Setup()
        {
            _scheme = new Mock<IScheme<SchemeParametersBase<KasDsaAlgoAttributesFfc>, KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>>();
            _scheme.Setup(s => s.SetDomainParameters(It.IsAny<FfcDomainParameters>()));
            _scheme
                .Setup(s => s.ReturnPublicInfoThisParty())
                .Returns(It.IsAny<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>>());
            _scheme
                .Setup(s => s.ComputeResult(It.IsAny<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>>()))
                .Returns(new KasResult(string.Empty));

            _subject = new Kas<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>(_scheme.Object);
        }

        [Test]
        public void ShouldInvokeSchemeSetDomainParameters()
        {
            _subject.SetDomainParameters(It.IsAny<FfcDomainParameters>());
            _scheme.Verify(v => v.SetDomainParameters(It.IsAny<FfcDomainParameters>()), 
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
            _subject.ComputeResult(It.IsAny<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>>());
            _scheme.Verify(v => v.ComputeResult(It.IsAny<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>>()),
                Times.Once,
                nameof(_scheme.Object.ComputeResult)
            );
        }
    }
}
