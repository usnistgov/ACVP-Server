using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        private TestDataMother _tdm = new TestDataMother();

        [Test]
        public void ShouldReconstituteTestGroupFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer );
            Assert.IsNotNull(subject);
           
        }

        [Test]
        public void ShouldSetProperTestTypeFromDynamicAnswer()
        {
            var sourceAnswer = GetSourceAnswer();
            var subject = new TestGroup(sourceAnswer);
            Assume.That(subject != null);
            Assert.AreEqual(sourceAnswer.testType, subject.TestType);
        }


        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "1");
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("NumberOfKeys", "13")]
        [TestCase("testType", "Monte Carlo")]
        [TestCase("NumberOfKEYs", "130")]
        public void ShouldReturnSetStringName(string name, string value)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, value);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("Fredo")]
        [TestCase("A5")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnparsableValues(string value)
        {
            var subject = new TestGroup();
            var result = subject.SetString("numberOfkeys", value);
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnFalseIfMergeFails()
        {
            Random800_90 rand = new Random800_90();
            var testCase = new TestCase()
            {
               
                Key = rand.GetRandomBitString(8),
                CipherText = null,
                PlainText = null,
                TestCaseId = 42
            };

            List<ITestCase> testCases = new List<ITestCase>
            {
                testCase
            };

            TestGroup tg = new TestGroup()
            {
                Tests = testCases
            };

            var result = tg.MergeTests(testCases);

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.IsFalse(result);
        }

       

        private dynamic GetSourceAnswer()
        {
            var sourceVector = new TestVectorSet() {TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup) g).ToList()};
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
