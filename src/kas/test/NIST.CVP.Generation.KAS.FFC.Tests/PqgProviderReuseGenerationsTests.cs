using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class PqgProviderReuseGenerationsTests
    {
        private PqgProviderReuseGenerations _subject;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private Mock<IDsaFfc> _dsa;

        private FfcDomainParameters _pqgDomainParameters;
        private FfcDomainParametersGenerateResult _genResult;

        [SetUp]
        public void Setup()
        {
            _pqgDomainParameters = new FfcDomainParameters(1, 2, 3);
            _genResult = new FfcDomainParametersGenerateResult(
                _pqgDomainParameters,
                new DomainSeed(1),
                new Counter(1)
            );

            _dsa = new Mock<IDsaFfc>();
            _dsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(_genResult);

            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
                .Returns(_dsa.Object);

            _subject = new PqgProviderReuseGenerations(_dsaFactory.Object);
        }

        [Test]
        public void ShouldInvokeDsaWhenKeyDoesNotExist()
        {
            _subject.GetPqg(1, 1, new HashFunction(ModeValues.SHA2, DigestSizes.d224));

            _dsa.Verify(v => v.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()), 
                Times.Once,
                nameof(_dsa.Object.GenerateDomainParameters)
            );
        }

        [Test]
        public void ShouldInvokeDsaWhenKeyExistOnlyFirstTime()
        {
            _subject.GetPqg(2, 2, new HashFunction(ModeValues.SHA2, DigestSizes.d224));
            _subject.GetPqg(2, 2, new HashFunction(ModeValues.SHA2, DigestSizes.d224));

            _dsa.Verify(v => v.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()),
                Times.Once,
                nameof(_dsa.Object.GenerateDomainParameters)
            );
        }
    }
}