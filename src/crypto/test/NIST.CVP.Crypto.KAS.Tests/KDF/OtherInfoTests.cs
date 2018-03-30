using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KDF
{
    [TestFixture,  FastCryptoTest]
    public class OtherInfoTests
    {
        private OtherInfo _subject;
        
        private static object[] _proofOfConceptTests = new object[]
        {
            new object[]
            {
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("CAFECAFE"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                64,
                "literal[12345abc]||uPartyInfo",
                new BitString("12345abccafecafe")
            },
            new object[]
            {
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("CAFECAFE"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                64,
                "uPartyInfo||literal[12345abc]",
                new BitString("cafecafe12345abc")
            },
            new object[]
            {
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("CAFECAFE"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString("faceface"), new BitString(0), new BitString(0)),
                96,
                "literal[12345abc]||uPartyInfo",
                new BitString("12345abccafecafefaceface")
            },
        };

        [Test]
        [TestCaseSource(nameof(_proofOfConceptTests))]
        public void OtherInfoProofOfConceptTests(OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> uPartySharedInformation, int otherInfoLength, string otherInfoPattern, BitString expectedBitString)
        {
            var vPartySharedInformation = new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(
                null, 
                new BitString(0), 
                new FfcKeyPair(0),
                new FfcKeyPair(0),
                new BitString(0),
                new BitString(0), 
                new BitString(0)
            );

            var uPartyOtherInfo = new PartyOtherInfo(uPartySharedInformation.PartyId, uPartySharedInformation.DkmNonce);
            var vPartyotherInfo = new PartyOtherInfo(vPartySharedInformation.PartyId, vPartySharedInformation.DkmNonce);

            _subject = new OtherInfo(
                new EntropyProvider(new Random800_90()), 
                otherInfoPattern, 
                otherInfoLength, 
                KeyAgreementRole.InitiatorPartyU, 
                uPartyOtherInfo, 
                vPartyotherInfo
            );
            
            var result = _subject.GetOtherInfo();

            Assert.AreEqual(expectedBitString.ToHex(), result.ToHex());
        }

        private static object[] _cavsTests = new object[]
        {
            new object[]
            {
                // iutRole
                KeyAgreementRole.InitiatorPartyU,
                // uParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("a1b2c3d4e5"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)), 
                // vParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("434156536964"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                // otherInfoLength
                240,
                // expectedOi
                new BitString("a1b2c3d4e54341565369646cfd9fa9ec70ae7f9b0d17cc63ea2103fbaf6b"), 
            },
            new object[]
            {
                // iutRole
                KeyAgreementRole.InitiatorPartyU,
                // uParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("a1b2c3d4e5"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)), 
                // vParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("434156536964"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                // otherInfoLength
                240,
                // expectedOi
                new BitString("a1b2c3d4e5434156536964715de49cfcefd2279e82ec9847d746cb31ca93"),
            },
            new object[]
            {
                // iutRole
                KeyAgreementRole.ResponderPartyV,
                // uParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("434156536964"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)), 
                // vParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("a1b2c3d4e5"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                // otherInfoLength
                240,
                // expectedOi
                new BitString("434156536964a1b2c3d4e52009a24c98c92d8a86461fbb212c8193db7a69"),
            },
            new object[]
            {
                // iutRole
                KeyAgreementRole.ResponderPartyV,
                // uParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("434156536964"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)), 
                // vParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("a1b2c3d4e5"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                // otherInfoLength
                240,
                // expectedOi
                new BitString("434156536964a1b2c3d4e53955b6e50d671741c61b9ffdd3764eed468bfc"),
            },
            new object[]
            {
                // iutRole
                KeyAgreementRole.InitiatorPartyU,
                // uParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("a1b2c3d4e5"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString("7e4710fc503b32d44b01f973d281"), new BitString(0), new BitString(0)), 
                // vParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("434156536964"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)),
                // otherInfoLength
                240,
                // expectedOi
                new BitString("a1b2c3d4e57e4710fc503b32d44b01f973d28143415653696440fd7c8f94"),
            },
            new object[]
            {
                // iutRole
                KeyAgreementRole.ResponderPartyV,
                // uParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("434156536964"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString("be58b39ab2f8ab722acac7a635f2"), new BitString(0), new BitString(0)),
                // vParty
                new OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>(null, new BitString("a1b2c3d4e5"), new FfcKeyPair(0), new FfcKeyPair(0), new BitString(0), new BitString(0), new BitString(0)), 
                // otherInfoLength
                240,
                // expectedOi
                new BitString("434156536964be58b39ab2f8ab722acac7a635f2a1b2c3d4e53d96b0e008"),
            },
        };

        [Test, FastCryptoTest]
        [TestCaseSource(nameof(_cavsTests))]
        public void ShouldReturnCorrectOtherInfoCavsTests(
            KeyAgreementRole iutRole, 
            OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> uPartySharedInformation, 
            OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair> vPartySharedInformation, 
            int otherInfoLength, 
            BitString expectedOtherInfo
        )
        {
            string otherInfoPattern = "uPartyInfo||vPartyInfo";

            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            // u/v party info comprised of ID, and dkmNonce, find the bitlength of both parties contributed information 
            // to determine which bits are the "random" bits to inject into the TestableEntropyProvider.
            var composedBitLength = uPartySharedInformation.PartyId.BitLength +
                            uPartySharedInformation.DkmNonce.BitLength +
                            vPartySharedInformation.PartyId.BitLength;

            var entropyBits = expectedOtherInfo.GetLeastSignificantBits(expectedOtherInfo.BitLength - composedBitLength);

            entropyProvider.AddEntropy(entropyBits);

            var uPartyOtherInfo = new PartyOtherInfo(uPartySharedInformation.PartyId, uPartySharedInformation.DkmNonce);
            var vPartyOtherInfo = new PartyOtherInfo(vPartySharedInformation.PartyId, vPartySharedInformation.DkmNonce);

            _subject = new OtherInfo(
                entropyProvider, 
                otherInfoPattern, 
                otherInfoLength, 
                KeyAgreementRole.InitiatorPartyU, 
                uPartyOtherInfo, 
                vPartyOtherInfo
            );

            var result = _subject.GetOtherInfo();

            Assert.AreEqual(expectedOtherInfo.ToHex(), result.ToHex());
        }
    }
}
