using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.Symmetric.Tests.Aes
{
    [TestFixture, FastCryptoTest]
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

            MCTResult<AlgoArrayResponse> subject = new MCTResult<AlgoArrayResponse>(list);

            Assert.AreEqual(numberOfResponses, subject.Response.Count);
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();
            MCTResult<AlgoArrayResponse> subject = new MCTResult<AlgoArrayResponse>(list);

            Assert.IsTrue(subject.Success, nameof(subject.Success));
            Assert.IsTrue(string.IsNullOrEmpty(subject.ErrorMessage), nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            MCTResult<AlgoArrayResponse> subject = new MCTResult<AlgoArrayResponse>(error);

            Assert.IsFalse(subject.Success, nameof(subject.Success));
            Assert.AreEqual(error, subject.ErrorMessage);
        }
    }
}
