using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Ansx;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pkcs;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures.Pss;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests.Signatures
{
    [TestFixture, FastCryptoTest]
    public class PaddingFactoryTests
    {
        [Test]
        [TestCase(SignatureSchemes.Ansx931, RSASignatureModifications.None, typeof(AnsxPadder))]
        [TestCase(SignatureSchemes.Ansx931, RSASignatureModifications.E, typeof(AnsxPadderWithModifiedPublicExponent))]
        [TestCase(SignatureSchemes.Ansx931, RSASignatureModifications.Message, typeof(AnsxPadderWithModifiedMessage))]
        [TestCase(SignatureSchemes.Ansx931, RSASignatureModifications.ModifyTrailer, typeof(AnsxPadderWithModifiedTrailer))]
        [TestCase(SignatureSchemes.Ansx931, RSASignatureModifications.MoveIr, typeof(AnsxPadderWithMovedIr))]
        [TestCase(SignatureSchemes.Ansx931, RSASignatureModifications.Signature, typeof(AnsxPadderWithModifiedSignature))]

        [TestCase(SignatureSchemes.Pkcs1v15, RSASignatureModifications.None, typeof(PkcsPadder))]
        [TestCase(SignatureSchemes.Pkcs1v15, RSASignatureModifications.E, typeof(PkcsPadderWithModifiedPublicExponent))]
        [TestCase(SignatureSchemes.Pkcs1v15, RSASignatureModifications.Message, typeof(PkcsPadderWithModifiedMessage))]
        [TestCase(SignatureSchemes.Pkcs1v15, RSASignatureModifications.ModifyTrailer, typeof(PkcsPadderWithModifiedTrailer))]
        [TestCase(SignatureSchemes.Pkcs1v15, RSASignatureModifications.MoveIr, typeof(PkcsPadderWithMovedIr))]
        [TestCase(SignatureSchemes.Pkcs1v15, RSASignatureModifications.Signature, typeof(PkcsPadderWithModifiedSignature))]

        [TestCase(SignatureSchemes.Pss, RSASignatureModifications.None, typeof(PssPadder))]
        [TestCase(SignatureSchemes.Pss, RSASignatureModifications.E, typeof(PssPadderWithModifiedPublicExponent))]
        [TestCase(SignatureSchemes.Pss, RSASignatureModifications.Message, typeof(PssPadderWithModifiedMessage))]
        [TestCase(SignatureSchemes.Pss, RSASignatureModifications.ModifyTrailer, typeof(PssPadderWithModifiedTrailer))]
        [TestCase(SignatureSchemes.Pss, RSASignatureModifications.MoveIr, typeof(PssPadderWithMovedIr))]
        [TestCase(SignatureSchemes.Pss, RSASignatureModifications.Signature, typeof(PssPadderWithModifiedSignature))]
        public void ShouldReturnCorrectPaddingSchemeWithErrors(SignatureSchemes sigScheme, RSASignatureModifications mods, Type expectedType)
        {
            var paddingFactory = new PaddingFactory(new MaskFactory(new NativeShaFactory()));
            var result = paddingFactory.GetSigningPaddingScheme(sigScheme, new NativeShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160)), mods, PssMaskTypes.MGF1);

            Assert.That(result != null);
            Assert.That(result, Is.InstanceOf(expectedType));
        }
    }
}
