using NIST.CVP.Generation.CMAC.TDES;
using NIST.CVP.Generation.CMAC.TDES.Parsers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using TestCase = NIST.CVP.Generation.CMAC.TDES.TestCase;
using TestGroup = NIST.CVP.Generation.CMAC.TDES.TestGroup;
using TestVectorSet = NIST.CVP.Generation.CMAC.TDES.TestVectorSet;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsTdes : FireHoseTestsBase<LegacyResponseFileParser, TestCaseGeneratorGen, TestVectorSet, TestGroup, TestCase>
    {
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidateTdes()
        {
            ShouldRunThroughAllTestFilesAndValidate();
        }

        protected override string FolderName => "TDES";

    }
}


