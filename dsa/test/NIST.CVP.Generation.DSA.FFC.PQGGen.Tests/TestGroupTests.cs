using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
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

        //[Test]
        //public void ShouldSetProperTestTypeFromDynamicAnswer()
        //{
        //    var sourceAnswer = GetSourceAnswer();
        //    var subject = new TestGroup(sourceAnswer);
        //    Assume.That(subject != null);
        //    Assert.AreEqual(sourceAnswer.testType, subject.TestType);
        //}

        //[Test]
        //public void ShouldSetProperSigGenModeFromDynamicAnswer()
        //{
        //    var sourceAnswer = GetSourceAnswer();
        //    var subject = new TestGroup(sourceAnswer);
        //    Assume.That(subject != null);
        //    Assert.AreEqual(sourceAnswer.sigType, RSAEnumHelpers.SigGenModeToString(subject.Mode));
        //}

        //[Test]
        //public void ShouldSetProperHashAlgFromDynamicAnswer()
        //{
        //    var sourceAnswer = GetSourceAnswer();
        //    var subject = new TestGroup(sourceAnswer);
        //    Assume.That(subject != null);
        //    Assert.AreEqual(sourceAnswer.hashAlg, SHAEnumHelpers.HashFunctionToString(subject.HashAlg));
        //}

        //[Test]
        //public void ShouldSetProperModuloFromDynamicAnswer()
        //{
        //    var sourceAnswer = GetSourceAnswer();
        //    var subject = new TestGroup(sourceAnswer);
        //    Assume.That(subject != null);
        //    Assert.AreEqual(sourceAnswer.modulo, subject.Modulo);
        //}

        private dynamic GetSourceAnswer()
        {
            var sourceVector = new TestVectorSet { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceAnswer = sourceVector.AnswerProjection[0];
            return sourceAnswer;
        }
    }
}
