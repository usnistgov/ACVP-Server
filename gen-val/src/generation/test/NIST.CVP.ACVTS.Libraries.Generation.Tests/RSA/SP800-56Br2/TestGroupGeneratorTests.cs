using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SP800_56Br2
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        [Test]
        [TestCase(new [] { 2048 })]
        [TestCase(new [] { 2048, 3072 })]
        [TestCase(new [] { 2048, 3072, 4096 })]
        public async Task ShouldCreateCorrectNumberOfTestGroupsByModulus(int[] modulus)
        {
            var parameters = new Parameters
            {
                IsSample = true,
                Modulo = modulus,
                KeyFormat = new [] { PrivateKeyModes.Standard },
                PublicExponentMode = PublicExponentModes.Random
            };

            var tgGen = new TestGroupGenerator();
            var groups = await tgGen.BuildTestGroupsAsync(parameters);

            Assert.That(groups.Count, Is.EqualTo(modulus.Length), "modulus");
        }
        
        [Test]
        [TestCase(new [] { PrivateKeyModes.Standard })]
        [TestCase(new [] { PrivateKeyModes.Crt })]
        [TestCase(new [] { PrivateKeyModes.Standard, PrivateKeyModes.Crt })]
        public async Task ShouldCreateCorrectNumberOfTestGroupsByKeyFormat(PrivateKeyModes[] keyFormats)
        {
            var parameters = new Parameters
            {
                IsSample = true,
                Modulo = new [] { 2048 },
                KeyFormat = keyFormats,
                PublicExponentMode = PublicExponentModes.Random
            };

            var tgGen = new TestGroupGenerator();
            var groups = await tgGen.BuildTestGroupsAsync(parameters);

            Assert.That(groups.Count, Is.EqualTo(keyFormats.Length * parameters.Modulo.Length), "keyFormats");
        }
    }
}
