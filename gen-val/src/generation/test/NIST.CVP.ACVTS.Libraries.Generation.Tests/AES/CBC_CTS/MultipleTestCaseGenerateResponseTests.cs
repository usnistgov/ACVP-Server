using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CBC_CTS
{
    [TestFixture, UnitTest]
    public class MultipleTestCaseGenerateResponseTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldContainResultsInResponseFromConstructor(int numberOfResponses)
        {
            List<TestCase> list = new List<TestCase>();
            for (int i = 0; i < numberOfResponses; i++)
            {
                list.Add(new TestCase());
            }

            var subject = new MultipleTestCaseGenerateResponse<TestGroup, TestCase>(list);

            Assert.AreEqual(numberOfResponses, subject.TestCases.Count());
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            List<TestCase> list = new List<TestCase>();
            var subject = new MultipleTestCaseGenerateResponse<TestGroup, TestCase>(list);

            Assert.IsTrue(subject.Success, nameof(subject.Success));
            Assert.IsTrue(string.IsNullOrEmpty(subject.ErrorMessage), nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            var subject = new MultipleTestCaseGenerateResponse<TestGroup, TestCase>(error);

            Assert.IsFalse(subject.Success, nameof(subject.Success));
            Assert.AreEqual(error, subject.ErrorMessage);
        }
    }
}
