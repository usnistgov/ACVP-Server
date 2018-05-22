using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KAS.Enums;
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
        [TestCase("AFT", FfcScheme.DhHybrid1, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhHybrid1, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhHybrid1, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.DhHybrid1, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhHybrid1, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhHybrid1, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", FfcScheme.Mqv2, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.Mqv2, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", FfcScheme.Mqv2, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.Mqv2, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.Mqv2, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.Mqv2, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", FfcScheme.DhEphem, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhEphem, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("VAL", FfcScheme.DhEphem, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhEphem, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", FfcScheme.DhHybridOneFlow, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhHybridOneFlow, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhHybridOneFlow, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.DhHybridOneFlow, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhHybridOneFlow, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhHybridOneFlow, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", FfcScheme.Mqv1, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.Mqv1, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", FfcScheme.Mqv1, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.Mqv1, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.Mqv1, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.Mqv1, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", FfcScheme.DhOneFlow, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhOneFlow, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        //[TestCase("AFT", FfcScheme.DhOneFlow, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.DhOneFlow, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhOneFlow, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        //[TestCase("VAL", FfcScheme.DhOneFlow, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", FfcScheme.DhStatic, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhStatic, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", FfcScheme.DhStatic, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", FfcScheme.DhStatic, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhStatic, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", FfcScheme.DhStatic, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        public void ShouldReturnCorrectValidator(string testType, FfcScheme scheme, KasMode kasMode, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType, Type expectedValidatorType)
        {
            TestVectorSet testVectorSet = null;
            GetData(testType, scheme, kasMode, kcRole, kcType, ref testVectorSet);

            var result = _subject.GetValidators(testVectorSet).ToList();

            Assume.That(result.Count == 1);
            Assert.IsInstanceOf(expectedValidatorType, result[0]);
        }

        private void GetData(string testType, FfcScheme scheme, KasMode kasMode, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType, ref TestVectorSet testVectorSet)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<TestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = testType,
                        KasMode = kasMode,
                        Scheme = scheme,
                        KcRole = kcRole,
                        KcType = kcType,
                        Tests = new List<TestCase>()
                        {
                            new TestCase()
                        }
                    }
                }
            };
        }
    }
}