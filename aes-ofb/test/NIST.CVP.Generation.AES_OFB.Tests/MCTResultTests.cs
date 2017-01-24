using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture]
    public class MCTResultTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldContainResultsInResponseFromConstructor(int numberOfResponses)
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();
            for (int i = 0; i < numberOfResponses; i++)
            {
                list.Add(new AlgoArrayResponse());
            }

            MCTResult subject = new MCTResult(list);

            Assert.AreEqual(numberOfResponses, subject.Response.Count);
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();
            MCTResult subject = new MCTResult(list);

            Assert.IsTrue(subject.Success, nameof(subject.Success));
            Assert.IsTrue(string.IsNullOrEmpty(subject.ErrorMessage), nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            MCTResult subject = new MCTResult(error);

            Assert.IsFalse(subject.Success, nameof(subject.Success));
            Assert.AreEqual(error, subject.ErrorMessage);
        }
    }
}
