using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KeyWrap.TDES;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
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
