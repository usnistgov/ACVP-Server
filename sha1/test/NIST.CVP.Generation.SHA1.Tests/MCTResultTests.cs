using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
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
            var list = new List<AlgoArrayResponse>();
            for(int i = 0; i < numberOfResponses; i++)
            {
                list.Add(new AlgoArrayResponse());
            }

            var subject = new MCTResult(list);

            Assert.AreEqual(numberOfResponses, subject.Response.Count);
        }

        [Test]
        public void ShouldBeSuccessfulWhenResponsesAssignedToInstance()
        {
            var list = new List<AlgoArrayResponse>();
            var subject = new MCTResult(list);

            Assert.IsTrue(subject.Success, nameof(subject.Success));
            Assert.IsTrue(string.IsNullOrEmpty(subject.ErrorMessage), nameof(subject.ErrorMessage));
        }

        [Test]
        public void ShouldBeNotSuccessfulOnError()
        {
            string error = "Error!";
            var subject = new MCTResult(error);

            Assert.IsFalse(subject.Success, nameof(subject.Success));
            Assert.AreEqual(error, subject.ErrorMessage);
        }
    }
}
