﻿using System.Text;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.Helpers
{
    [TestFixture,  FastCryptoTest]
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

        [Test]
        public void ShouldGetGenerationRequirementsFfc()
        {
            var headers = KeyGenerationRequirementsHelper.FfcSchemeKeyGenerationRequirements[0];

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<texttable anchor=""scheme_generation_requirements"" title=""Required Party Generation Obligations"">");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.Scheme)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.KasMode)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.ThisPartyKasRole)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.ThisPartyKeyConfirmationRole)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.KeyConfirmationDirection)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesStaticKeyPair)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesEphemeralKeyPair)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesEphemeralNonce)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesDkmNonce)}</ttcol>");
            sb.AppendLine();
            foreach (var item in KeyGenerationRequirementsHelper.FfcSchemeKeyGenerationRequirements)
            {
                sb.Append($@"<c>{item.Scheme}</c>");
                sb.Append($@"<c>{item.KasMode}</c>");
                sb.Append($@"<c>{item.ThisPartyKasRole}</c>");
                sb.Append($@"<c>{item.ThisPartyKeyConfirmationRole}</c>");
                sb.Append($@"<c>{item.KeyConfirmationDirection}</c>");
                sb.Append($@"<c>{item.GeneratesStaticKeyPair}</c>");
                sb.Append($@"<c>{item.GeneratesEphemeralKeyPair}</c>");
                sb.Append($@"<c>{item.GeneratesEphemeralNonce}</c>");
                sb.Append($@"<c>{item.GeneratesDkmNonce}</c>");
                sb.Append($@"<c/><c/><c/><c/><c/><c/><c/><c/><c/>");
                sb.AppendLine();
            }

            sb.Append("</texttable>");

            var s = sb.ToString();

            Assert.IsTrue(true);
        }

        [Test]
        public void ShouldGetGenerationRequirementsEcc()
        {
            var headers = KeyGenerationRequirementsHelper.EccSchemeKeyGenerationRequirements[0];

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<texttable anchor=""scheme_generation_requirements"" title=""Required Party Generation Obligations"">");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.Scheme)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.KasMode)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.ThisPartyKasRole)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.ThisPartyKeyConfirmationRole)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.KeyConfirmationDirection)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesStaticKeyPair)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesEphemeralKeyPair)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesEphemeralNonce)}</ttcol>");
            sb.Append($@"<ttcol align=""left"">{nameof(headers.GeneratesDkmNonce)}</ttcol>");
            sb.AppendLine();
            foreach (var item in KeyGenerationRequirementsHelper.FfcSchemeKeyGenerationRequirements)
            {
                sb.Append($@"<c>{item.Scheme}</c>");
                sb.Append($@"<c>{item.KasMode}</c>");
                sb.Append($@"<c>{item.ThisPartyKasRole}</c>");
                sb.Append($@"<c>{item.ThisPartyKeyConfirmationRole}</c>");
                sb.Append($@"<c>{item.KeyConfirmationDirection}</c>");
                sb.Append($@"<c>{item.GeneratesStaticKeyPair}</c>");
                sb.Append($@"<c>{item.GeneratesEphemeralKeyPair}</c>");
                sb.Append($@"<c>{item.GeneratesEphemeralNonce}</c>");
                sb.Append($@"<c>{item.GeneratesDkmNonce}</c>");
                sb.Append($@"<c/><c/><c/><c/><c/><c/><c/><c/><c/>");
                sb.AppendLine();
            }

            sb.Append("</texttable>");

            var s = sb.ToString();

            Assert.IsTrue(true);
        }
    }
}