using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TPMv1_2;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TPM
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        [Test]
        public async Task ShouldCreateATestGroupForEachCombinationOfVersionAndHash()
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(new ParameterBuilder().Build());
            Assert.AreEqual(1, results.Count());
        }
    }
}
