using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Generation.CMAC.AES.Parsers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsAes : FireHoseTestsBase<LegacyResponseFileParser, TestCaseGeneratorGen, TestVectorSet, TestGroup, TestCase>
    {
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidateAes()
        {
            ShouldRunThroughAllTestFilesAndValidate();
        }

        protected override string FolderName => "AES";

    }
}


