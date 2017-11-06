using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class KeyGenerationRequirementsHelperTests
    {
        [Test]
        [TestCase(KeyAgreementRole.InitiatorPartyU, KeyAgreementRole.ResponderPartyV)]
        [TestCase(KeyAgreementRole.ResponderPartyV, KeyAgreementRole.InitiatorPartyU)]
        public void ShouldReturnCorrectKasRole(KeyAgreementRole rolePartyA, KeyAgreementRole expectedRolePartyB)
        {
            var result = KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(rolePartyA);

            Assert.AreEqual(expectedRolePartyB, result);
        }

        [Test]
        [TestCase(KeyConfirmationRole.None, KeyConfirmationRole.None)]
        [TestCase(KeyConfirmationRole.Provider, KeyConfirmationRole.Recipient)]
        [TestCase(KeyConfirmationRole.Recipient, KeyConfirmationRole.Provider)]
        public void ShouldReturnCorrectKcRole(KeyConfirmationRole rolePartyA, KeyConfirmationRole expectedRolePartyB)
        {
            var result = KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(rolePartyA);

            Assert.AreEqual(expectedRolePartyB, result);
        }
    }
}