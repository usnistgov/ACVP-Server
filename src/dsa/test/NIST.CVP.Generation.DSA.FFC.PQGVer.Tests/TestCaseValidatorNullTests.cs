using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorNullTests
    {
        [Test]
        public async Task ShouldReturnFailedForInitialValidate()
        {
            var subject = new TestCaseValidatorNull("error", 0);
            var result = await subject.ValidateAsync(new TestCase());
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }
    }
}
