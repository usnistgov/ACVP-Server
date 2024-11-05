using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Tests.Aes
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

            Assert.That(subject.Response.Count, Is.EqualTo(numberOfResponses));
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();
            MCTResult<AlgoArrayResponse> subject = new MCTResult<AlgoArrayResponse>(list);

            Assert.That(subject.Success, Is.True, nameof(subject.Success));
            Assert.That(string.IsNullOrEmpty(subject.ErrorMessage), Is.True, nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            MCTResult<AlgoArrayResponse> subject = new MCTResult<AlgoArrayResponse>(error);

            Assert.That(subject.Success, Is.False, nameof(subject.Success));
            Assert.That(subject.ErrorMessage, Is.EqualTo(error));
        }
    }
}
