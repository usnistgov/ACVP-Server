using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Crypto.TupleHash;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.ParallelHash.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class RunGeneratedTestThroughCrypto
    {
        private IOracle _oracle = new OracleBuilder().Build().GetAwaiter().GetResult();

        [OneTimeTearDown]
        public async Task Teardown()
        {
            await _oracle.CloseClusterClient();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGenerateSameResultHexCustomization(bool xof)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 8));

            var hashFunction = new HashFunction(256, 256, xof);

            var resultFromOracle = await _oracle.GetParallelHashCaseAsync(new ParallelHashParameters()
            {
                CustomizationLength = 128,
                HashFunction = hashFunction,
                HexCustomization = true,
                MessageLength = 256,
                BlockSize = 256,
                OutLens = minMax.GetDeepCopy()
            });

            var hash = new ParallelHashFactory().GetParallelHash(hashFunction);
            var directCryptoResult = hash.HashMessage(
                resultFromOracle.Message,
                256, 256, 256, xof, resultFromOracle.CustomizationHex);

            var secondCryptoResultWithoutCustomization = hash.HashMessage(
                resultFromOracle.Message,
                256, 256, 256, xof);

            Assert.AreEqual(resultFromOracle.Digest.ToHex(), directCryptoResult.ToHex());
            Assert.AreNotEqual(directCryptoResult.ToHex(), secondCryptoResultWithoutCustomization.ToHex());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGenerateSameResultNoHexCustomization(bool xof)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 8));

            var hashFunction = new HashFunction(256, 256, xof);

            var resultFromOracle = await _oracle.GetParallelHashCaseAsync(new ParallelHashParameters()
            {
                CustomizationLength = 128,
                HashFunction = hashFunction,
                HexCustomization = false,
                MessageLength = 256,
                BlockSize = 256,
                OutLens = minMax.GetDeepCopy()
            });

            var hash = new ParallelHashFactory().GetParallelHash(hashFunction);
            var directCryptoResult = hash.HashMessage(
                resultFromOracle.Message,
                256, 256, 256, xof, resultFromOracle.Customization);

            var secondCryptoResultWithoutCustomization = hash.HashMessage(
                resultFromOracle.Message,
                256, 256, 256, xof);

            Assert.AreEqual(resultFromOracle.Digest.ToHex(), directCryptoResult.ToHex());
            Assert.AreNotEqual(directCryptoResult.ToHex(), secondCryptoResultWithoutCustomization.ToHex());
        }
    }
}
