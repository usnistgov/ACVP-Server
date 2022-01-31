using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.TDES;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTestsTdes : FireHoseTestsBase<TestVectorSet, TestGroup, TestCase>
    {
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidateTdes()
        {
            ShouldRunThroughAllTestFilesAndValidate();
        }

        protected override string FolderName => "TDES";
    }
}
