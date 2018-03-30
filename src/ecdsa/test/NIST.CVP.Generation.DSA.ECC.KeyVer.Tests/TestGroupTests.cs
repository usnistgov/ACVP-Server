using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldReconstituteTestGroupFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assert.IsNotNull(subject);
        }

        private dynamic GetSourceAnswer()
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
