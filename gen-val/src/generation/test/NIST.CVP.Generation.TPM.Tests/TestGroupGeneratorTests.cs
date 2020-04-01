using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.KDF_Components.v1_0.TPMv1_2;

namespace NIST.CVP.Generation.TPM.Tests
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
