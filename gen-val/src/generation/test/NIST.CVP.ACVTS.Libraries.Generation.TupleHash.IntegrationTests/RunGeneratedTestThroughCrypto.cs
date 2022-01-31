using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.TupleHash;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TupleHash.IntegrationTests
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

            var resultFromOracle = await _oracle.GetTupleHashCaseAsync(new TupleHashParameters()
            {
                CustomizationLength = 128,
                FunctionName = "TupleHash",
                HashFunction = hashFunction,
                HexCustomization = true,
                MessageLength = new[] { 256 },
                OutputLengths = minMax
            });

            var tupleHash = new TupleHashFactory().GetTupleHash(hashFunction);
            var directCryptoResult = tupleHash.HashMessage(
                resultFromOracle.Tuple, 256, 256, xof, resultFromOracle.CustomizationHex);

            var secondCryptoResultWithoutCustomization = tupleHash.HashMessage(
                resultFromOracle.Tuple, 256, 256, xof);

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

            var resultFromOracle = await _oracle.GetTupleHashCaseAsync(new TupleHashParameters()
            {
                CustomizationLength = 128,
                FunctionName = "TupleHash",
                HashFunction = hashFunction,
                HexCustomization = false,
                MessageLength = new[] { 256 },
                OutputLengths = minMax
            });

            var tupleHash = new TupleHashFactory().GetTupleHash(hashFunction);
            var directCryptoResult = tupleHash.HashMessage(
                resultFromOracle.Tuple, 256, 256, xof, resultFromOracle.Customization
            );

            var secondCryptoResultWithoutCustomization = tupleHash.HashMessage(
                resultFromOracle.Tuple, 256, 256, xof);

            Assert.AreEqual(resultFromOracle.Digest.ToHex(), directCryptoResult.ToHex());
            Assert.AreNotEqual(directCryptoResult.ToHex(), secondCryptoResultWithoutCustomization.ToHex());
        }
    }
}
