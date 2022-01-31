using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsAes : FireHoseTestsBase<LegacyResponseFileParserAes>
    {
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidateAes()
        {
            ShouldRunThroughAllTestFilesAndValidate();
        }

        protected override string FolderName => "AES";

    }
}


