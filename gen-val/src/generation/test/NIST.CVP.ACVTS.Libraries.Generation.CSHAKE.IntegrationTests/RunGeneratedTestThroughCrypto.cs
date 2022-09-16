using System.Threading.Tasks;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.IntegrationTests
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
            var functionName = "cSHAKE";
            var hashFunction = new HashFunction()
            {
                Capacity = 512,
                DigestLength = 256
            };

            var resultFromOracle = await _oracle.GetCShakeCaseAsync(new CShakeParameters()
            {
                CustomizationLength = 128,
                HexCustomization = true,
                MessageLength = 256,
                HashFunction = hashFunction,
                FunctionName = functionName
            });

            var hash = new cSHAKEFactory().GetcSHAKE(hashFunction);
            var result = hash.HashMessage(
                resultFromOracle.Message,
                256,
                512,
                resultFromOracle.CustomizationHex,
                functionName);

            Assert.AreEqual(resultFromOracle.Digest.ToHex(), result.ToHex());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldGenerateSameResultNoHexCustomization(bool xof)
        {
            var functionName = "cSHAKE";
            var hashFunction = new HashFunction()
            {
                Capacity = 512,
                DigestLength = 256
            };

            var resultFromOracle = await _oracle.GetCShakeCaseAsync(new CShakeParameters()
            {
                CustomizationLength = 128,
                HexCustomization = false,
                MessageLength = 256,
                HashFunction = hashFunction,
                FunctionName = functionName
            });

            var hash = new cSHAKEFactory().GetcSHAKE(hashFunction);
            var result = hash.HashMessage(
                resultFromOracle.Message,
                256,
                512,
                resultFromOracle.Customization,
                functionName);

            Assert.AreEqual(resultFromOracle.Digest.ToHex(), result.ToHex());
        }
    }
}
