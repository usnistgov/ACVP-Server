using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private readonly TestCaseValidatorFactory _subject = new TestCaseValidatorFactory(null, null, null, null, null);
        
        [Test]
        [TestCase("AFT", EccScheme.FullUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.FullUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", EccScheme.FullUnified, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", EccScheme.FullUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.FullUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.FullUnified, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", EccScheme.FullMqv, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.FullMqv, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", EccScheme.FullMqv, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", EccScheme.FullMqv, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.FullMqv, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.FullMqv, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", EccScheme.EphemeralUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.EphemeralUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("VAL", EccScheme.EphemeralUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.EphemeralUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", EccScheme.OnePassUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.OnePassUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", EccScheme.OnePassUnified, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", EccScheme.OnePassUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.OnePassUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.OnePassUnified, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", EccScheme.OnePassMqv, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.OnePassMqv, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", EccScheme.OnePassMqv, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", EccScheme.OnePassMqv, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.OnePassMqv, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.OnePassMqv, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", EccScheme.OnePassDh, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.OnePassDh, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        //[TestCase("AFT", EccScheme.OnePassDh, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", EccScheme.OnePassDh, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.OnePassDh, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        //[TestCase("VAL", EccScheme.OnePassDh, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]

        [TestCase("AFT", EccScheme.StaticUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftNoKdfNoKc))]
        [TestCase("AFT", EccScheme.StaticUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorAftKdfNoKc))]
        [TestCase("AFT", EccScheme.StaticUnified, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorAftKdfKc))]
        [TestCase("VAL", EccScheme.StaticUnified, KasMode.NoKdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.StaticUnified, KasMode.KdfNoKc, KeyConfirmationRole.None, KeyConfirmationDirection.None, typeof(TestCaseValidatorVal))]
        [TestCase("VAL", EccScheme.StaticUnified, KasMode.KdfKc, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, typeof(TestCaseValidatorVal))]
        public void ShouldReturnCorrectValidator(string testType, EccScheme scheme, KasMode kasMode, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType, Type expectedValidatorType)
        {
            TestVectorSet testVectorSet = null;
            GetData(testType, scheme, kasMode, kcRole, kcType, ref testVectorSet);

            var result = _subject.GetValidators(testVectorSet, null).ToList();

            Assume.That(result.Count == 1);
            Assert.IsInstanceOf(expectedValidatorType, result[0]);
        }

        private void GetData(string testType, EccScheme scheme, KasMode kasMode, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType, ref TestVectorSet testVectorSet)
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