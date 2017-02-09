using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
    public class KnownAnswerTestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldReturnResponseWithErrorMessage()
        {
            TestGroup testGroup = new TestGroup();

            KnownAnswerTestCaseGeneratorNull subject = new KnownAnswerTestCaseGeneratorNull();
            var result = subject.Generate(testGroup);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
