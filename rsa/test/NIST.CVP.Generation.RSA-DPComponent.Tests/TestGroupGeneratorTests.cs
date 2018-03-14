using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_DPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        [Test]
        [TestCase(true, 2, 6)]
        [TestCase(false, 10, 30)]
        public void ShouldCreateASingleGroupWithCorrectProperties(bool isSample, int failing, int total)
        {
            var parameters = new Parameters
            {
                IsSample = isSample
            };

            var subject = new TestGroupGenerator();
            var result = subject.BuildTestGroups(parameters);

            Assert.AreEqual(1, result.Count());

            var testGroup = result.First();
            Assert.AreEqual(failing, testGroup.TotalFailingCases, "failing");
            Assert.AreEqual(total, testGroup.TotalTestCases, "total");
        }
    }
}
