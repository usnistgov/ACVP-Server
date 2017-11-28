using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private readonly TestCaseValidatorFactory _subject = new TestCaseValidatorFactory(null, null, null, null);
        
        [Test]
        [TestCase("AFT", FfcScheme.DhEphem, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhEphem, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("VAL", FfcScheme.DhEphem, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhEphem, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        
        [TestCase("AFT", FfcScheme.Mqv1, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.Mqv1, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", FfcScheme.Mqv1, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.Mqv1, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.Mqv1, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.Mqv1, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]
        public void ShouldReturnCorrectValidator(string testType, FfcScheme scheme, KasMode kasMode, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType, Type expectedValidatorType)
        {
            TestVectorSet testVectorSet = null;
            GetData(testType, scheme, kasMode, kcRole, kcType, ref testVectorSet);

            var result = _subject.GetValidators(testVectorSet, null).ToList();

            Assume.That(result.Count == 1);
            Assert.IsInstanceOf(expectedValidatorType, result[0]);
        }

        private void GetData(string testType, FfcScheme scheme, KasMode kasMode, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType, ref TestVectorSet testVectorSet)
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
                        Scheme = scheme,
                        KcRole = kcRole,
                        KcType = kcType,
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