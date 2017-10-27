using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private readonly TestCaseValidatorFactory _subject = new TestCaseValidatorFactory(null, null, null, null);
        
        [Test]
        [TestCase("AFT", KasMode.NoKdfNoKc, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", KasMode.KdfNoKc, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("VAL", KasMode.NoKdfNoKc, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", KasMode.KdfNoKc, typeof(TestCaseValidatorVal))]
        public void ShouldReturnCorrectValidator(string testType, KasMode kasMode, Type expectedValidatorType)
        {
            TestVectorSet testVectorSet = null;
            GetData(testType, kasMode, ref testVectorSet);

            var result = _subject.GetValidators(testVectorSet, null).ToList();

            Assume.That(result.Count == 1);
            Assert.IsInstanceOf(expectedValidatorType, result[0]);
        }

        private void GetData(string testType, KasMode kasMode, ref TestVectorSet testVectorSet)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = testType,
                        KasMode = kasMode,
                        Scheme = FfcScheme.DhEphem,
                        Tests = new List<ITestCase>()
                        {
                            new TestCase()
                        }
                    }
                }
            };
        }
    }
}