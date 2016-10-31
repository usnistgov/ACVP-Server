using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class TestVectorSetTests
    {
        [Test]
        public void ShouldHaveTheExpectedAnswerProjection()
        {
            var subject = new TestVectorSet {Algorithm = "AES-GCM"};

            subject.TestGroups.Add(new TestGroup {AADLength = 16, Function = "encrypt", IVGeneration = "blah", IVGenerationMode = "internal", IVLength = 96, KeyLength = 256, PTLength = 256, TagLength = 16, Tests = new List<ITestCase> {new TestCase {AAD = new BitString("AAD"), TestCaseId = 1} } });
            var results = subject.AnswerProjection;
            Assert.IsNotNull(results);
        }
    }
}
