using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.TupleHash;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KMAC.IntegrationTests
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
            var resultFromOracle = await _oracle.GetKmacCaseAsync(new KmacParameters()
            {
                CustomizationLength = 128,
                HexCustomization = true,
                CouldFail = false,
                XOF = xof,
                DigestSize = 256,
                KeyLength = 256,
                MacLength = 256,
                MessageLength = 256
            });

            var mac = new KmacFactory(new cSHAKEWrapper()).GetKmacInstance(512, xof);
            var directCryptoResult = mac.Generate(
                resultFromOracle.Key,
                resultFromOracle.Message,
                resultFromOracle.CustomizationHex,
                256);

            Assert.AreEqual(resultFromOracle.Tag.ToHex(), directCryptoResult.Mac.ToHex());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGenerateSameResultNoHexCustomization(bool xof)
        {
            var resultFromOracle = await _oracle.GetKmacCaseAsync(new KmacParameters()
            {
                CustomizationLength = 128,
                HexCustomization = false,
                CouldFail = false,
                XOF = xof,
                DigestSize = 256,
                KeyLength = 256,
                MacLength = 256,
                MessageLength = 256
            });

            var mac = new KmacFactory(new cSHAKEWrapper()).GetKmacInstance(512, xof);
            var directCryptoResult = mac.Generate(
                resultFromOracle.Key,
                resultFromOracle.Message,
                resultFromOracle.Customization,
                256);

            Assert.AreEqual(resultFromOracle.Tag.ToHex(), directCryptoResult.Mac.ToHex());
        }
    }
}
