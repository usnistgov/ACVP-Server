using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class PqgProviderPreGeneratedTests
    {
        private PqgProviderPreGenerated _subject;
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

            _subject = new PqgProviderPreGenerated(_dsaFactory.Object);
        }

        private static object[] _testCasesValidPqgKeys = new object[]
        {
            new object[] { 2048, 224, new HashFunction(ModeValues.SHA2, DigestSizes.d224) },
            new object[] { 2048, 224, new HashFunction(ModeValues.SHA2, DigestSizes.d256) },
            new object[] { 2048, 224, new HashFunction(ModeValues.SHA2, DigestSizes.d384) },
            new object[] { 2048, 224, new HashFunction(ModeValues.SHA2, DigestSizes.d512) },
            new object[] { 2048, 256, new HashFunction(ModeValues.SHA2, DigestSizes.d256) },
            new object[] { 2048, 256, new HashFunction(ModeValues.SHA2, DigestSizes.d384) },
            new object[] { 2048, 256, new HashFunction(ModeValues.SHA2, DigestSizes.d512) },
        };

        [Test]
        [TestCaseSource(nameof(_testCasesValidPqgKeys))]
        public void ShouldNotInvokeDsaWhenKeyFound(int p, int q, HashFunction hashFunction)
        {
            _subject.GetPqg(p, q, hashFunction);

            _dsa.Verify(v => v.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()),
                Times.Never,
                nameof(_dsa.Object.GenerateDomainParameters)
            );
        }

        [Test]
        [TestCaseSource(nameof(_testCasesValidPqgKeys))]
        public void ShouldReturnAlreadyExistingPqgWhenGivenAppropriateKey(int p, int q, HashFunction hashFunction)
        {
            var result = _subject.GetPqg(p, q, hashFunction);

            Assert.AreNotEqual(_pqgDomainParameters, result);
        }

        [Test]
        public void ShouldInvokeDsaWhenKeyNotFound()
        {
            _subject.GetPqg(11, 11, new HashFunction(ModeValues.SHA2, DigestSizes.d224));

            _dsa.Verify(v => v.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()), 
                Times.Once,
                nameof(_dsa.Object.GenerateDomainParameters)
            );
        }

        [Test]
        public void ShouldReturnNewPqgWhenKeyNotFound()
        {
            var result = _subject.GetPqg(1, 1, new HashFunction(ModeValues.SHA2, DigestSizes.d224));

            Assert.AreEqual(_pqgDomainParameters, result);
        }
    }
}