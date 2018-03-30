using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorNullTests
    {
        [Test]
        public void ShouldReturnFailedForInitialValidate()
        {
            var subject = new TestCaseValidatorNull("error", 0);
            var result = subject.Validate(new TestCase());
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }
    }
}
