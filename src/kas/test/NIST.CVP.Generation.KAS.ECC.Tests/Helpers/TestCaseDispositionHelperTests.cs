using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.KAS.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class TestCaseDispositionHelperTests
    {
        [Test]
        [TestCase(EccScheme.EphemeralUnified, KasValTestDisposition.FailAssuranceServerStaticPublicKey, 0)]
        [TestCase(EccScheme.StaticUnified, KasValTestDisposition.FailAssuranceServerStaticPublicKey, 2)]
        public void ShouldNotIncludeStaticFailureConditionOnEphemeralOnlySchemes(EccScheme scheme,
            KasValTestDisposition disposition, int count)
        {
            var testGroup = new TestGroup()
            {
                Scheme = scheme,
                KasRole = KeyAgreementRole.InitiatorPartyU,
                KasMode = KasMode.NoKdfNoKc,
                Function = KasAssurance.PartialVal
            };

            var result = TestCaseDispositionHelper.PopulateValidityTestCaseOptions(testGroup);

            Assert.AreEqual(count, result.Count(c => c == disposition));
        }
    }
}
