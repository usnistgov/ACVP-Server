using NIST.CVP.Generation.TPMv1._2;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.TPM.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        [Test]
        public void ShouldCreateATestGroupForEachCombinationOfVersionAndHash()
        {
            var subject = new TestGroupGenerator();
            var results = subject.BuildTestGroups(new ParameterBuilder().Build());
            Assert.AreEqual(1, results.Count());
        }
    }
}
