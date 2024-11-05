using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CBC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CBC
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

            Assert.That(subject.TestCases.Count(), Is.EqualTo(numberOfResponses));
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            List<TestCase> list = new List<TestCase>();
            var subject = new MultipleTestCaseGenerateResponse<TestGroup, TestCase>(list);

            Assert.That(subject.Success, Is.True, nameof(subject.Success));
            Assert.That(string.IsNullOrEmpty(subject.ErrorMessage), Is.True, nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            var subject = new MultipleTestCaseGenerateResponse<TestGroup, TestCase>(error);

            Assert.That(subject.Success, Is.False, nameof(subject.Success));
            Assert.That(subject.ErrorMessage, Is.EqualTo(error));
        }
    }
}
