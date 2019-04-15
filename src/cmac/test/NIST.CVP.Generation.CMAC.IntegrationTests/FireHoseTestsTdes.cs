using NIST.CVP.Generation.CMAC.v1_0.Parsers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsTdes : FireHoseTestsBase<LegacyResponseFileParserTdes>
    {
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidateTdes()
        {
            ShouldRunThroughAllTestFilesAndValidate();
        }

        protected override string FolderName => "TDES";

    }
}


