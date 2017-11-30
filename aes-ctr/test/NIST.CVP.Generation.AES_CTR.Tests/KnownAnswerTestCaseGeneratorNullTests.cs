using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class KnownAnswerTestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldReturnResponseWithErrorMessage()
        {
            var testGroup = new TestGroup();

            var subject = new KnownAnswerTestCaseGeneratorNull();
            var result = subject.Generate(testGroup);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
