using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
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

            MultipleTestCaseGenerateResponse<IEnumerable<TestCase>> subject = new MultipleTestCaseGenerateResponse<IEnumerable<TestCase>>(list);

            Assert.AreEqual(numberOfResponses, subject.TestCases.Count());
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            List<TestCase> list = new List<TestCase>();
            MultipleTestCaseGenerateResponse<IEnumerable<TestCase>> subject = new MultipleTestCaseGenerateResponse<IEnumerable<TestCase>>(list);

            Assert.IsTrue(subject.Success, nameof(subject.Success));
            Assert.IsTrue(string.IsNullOrEmpty(subject.ErrorMessage), nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            MultipleTestCaseGenerateResponse<IEnumerable<TestCase>> subject = new MultipleTestCaseGenerateResponse<IEnumerable<TestCase>>(error);

            Assert.IsFalse(subject.Success, nameof(subject.Success));
            Assert.AreEqual(error, subject.ErrorMessage);
        }
    }
}
