using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests
{
    [TestFixture,  FastCryptoTest]
    public class KasTestsFfc
    {
        private Kas<FfcParameterSet, FfcScheme> _subject;
        private Mock<IScheme<SchemeParametersBase<FfcParameterSet, FfcScheme>, FfcParameterSet, FfcScheme>> _scheme;

        [SetUp]
        public void Setup()
        {
            _scheme = new Mock<IScheme<SchemeParametersBase<FfcParameterSet, FfcScheme>, FfcParameterSet, FfcScheme>>();
            _scheme.Setup(s => s.SetDomainParameters(It.IsAny<FfcDomainParameters>()));
            _scheme
                .Setup(s => s.ReturnPublicInfoThisParty())
                .Returns(It.IsAny<FfcSharedInformation>());
            _scheme
                .Setup(s => s.ComputeResult(It.IsAny<FfcSharedInformation>()))
                .Returns(new KasResult(string.Empty));

            _subject = new Kas<FfcParameterSet, FfcScheme>(_scheme.Object);
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
            _subject.ComputeResult(It.IsAny<FfcSharedInformation>());
            _scheme.Verify(v => v.ComputeResult(It.IsAny<FfcSharedInformation>()),
                Times.Once,
                nameof(_scheme.Object.ComputeResult)
            );
        }
    }
}
