using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsAes : FireHoseTestsBase<TestVectorSet, TestGroup, TestCase>
    {
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidateAes()
        {
            ShouldRunThroughAllTestFilesAndValidate();
        }

        protected override string FolderName => "AES";
    }
}
