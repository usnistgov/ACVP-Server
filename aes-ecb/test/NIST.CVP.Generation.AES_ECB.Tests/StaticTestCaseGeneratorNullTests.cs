using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
    public class StaticTestCaseGeneratorNullTests
    {
        public void ShouldReturnResponseWithErrorMessage()
        {
            TestGroup testGroup = new TestGroup();

            StaticTestCaseGeneratorNull subject = new StaticTestCaseGeneratorNull();
            var result = subject.Generate(testGroup);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
