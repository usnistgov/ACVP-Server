﻿using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.RSA.Signatures.Ansx;
using NIST.CVP.Crypto.RSA.Signatures.Pkcs;
using NIST.CVP.Crypto.RSA.Signatures.Pss;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests.Signatures
{
    [TestFixture, FastCryptoTest]
    public class PaddingFactoryTests
    {
        [Test]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.None, typeof(AnsxPadder))]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.E, typeof(AnsxPadderWithModifiedPublicExponent))]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.Message, typeof(AnsxPadderWithModifiedMessage))]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.ModifyTrailer, typeof(AnsxPadderWithModifiedTrailer))]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.MoveIr, typeof(AnsxPadderWithMovedIr))]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.Signature, typeof(AnsxPadderWithModifiedSignature))]

        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.None, typeof(PkcsPadder))]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.E, typeof(PkcsPadderWithModifiedPublicExponent))]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.Message, typeof(PkcsPadderWithModifiedMessage))]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.ModifyTrailer, typeof(PkcsPadderWithModifiedTrailer))]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.MoveIr, typeof(PkcsPadderWithMovedIr))]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.Signature, typeof(PkcsPadderWithModifiedSignature))]

        [TestCase(SignatureSchemes.Pss, SignatureModifications.None, typeof(PssPadder))]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.E, typeof(PssPadderWithModifiedPublicExponent))]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Message, typeof(PssPadderWithModifiedMessage))]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.ModifyTrailer, typeof(PssPadderWithModifiedTrailer))]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.MoveIr, typeof(PssPadderWithMovedIr))]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Signature, typeof(PssPadderWithModifiedSignature))]
        public void ShouldReturnCorrectPaddingSchemeWithErrors(SignatureSchemes sigScheme, SignatureModifications mods, Type expectedType)
        {
            var paddingFactory = new PaddingFactory(new MaskFactory(new ShaFactory()));
            var result = paddingFactory.GetSigningPaddingScheme(sigScheme, new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160)), mods, PssMaskTypes.MGF1);

            Assume.That(result != null);
            Assert.IsInstanceOf(expectedType, result);
        }
    }
}
