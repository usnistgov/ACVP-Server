using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KC
{
    public class OtherInfoProofOfConcept
    {

        public BitString DoStuff(OtherInfoPieces otherInfoPieces, string testPattern)
        {
            // split the pattern into pieces
            var pieces = testPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            BitString oi = new BitString(0);

            foreach (var piece in pieces)
            {
                var workingPiece = piece.Replace("||", "");
                ConcatenatePieceOntoOtherInfo(otherInfoPieces, workingPiece, ref oi);
            }

            return oi;
        }

        private void ConcatenatePieceOntoOtherInfo(OtherInfoPieces otherInfoPieces, string workingPiece, ref BitString oi)
        {
            if (workingPiece.Equals("uPartyId", StringComparison.OrdinalIgnoreCase))
            {
                oi = oi.ConcatenateBits(ReturnZeroLengthBitStringWhenNull(otherInfoPieces.UPartyId));
                oi = oi.ConcatenateBits(ReturnZeroLengthBitStringWhenNull(otherInfoPieces.DkmNonce));
                return;
            }
            if (workingPiece.StartsWith("literal[", StringComparison.OrdinalIgnoreCase))
            {
                // remove the "literal[]" to get just the hex value
                workingPiece = workingPiece.Replace("literal[", "").Replace("]", "");
                oi = oi.ConcatenateBits(new BitString(workingPiece));
                return;
            }

            throw new ArgumentException(nameof(workingPiece));
        }

        private BitString ReturnZeroLengthBitStringWhenNull(BitString bitstring)
        {
            if (bitstring == null)
                return new BitString(0);

            return bitstring;
        }
    }

    public class OtherInfoPieces
    {
        public OtherInfoPieces(BitString uPartyId)
        {
            UPartyId = uPartyId;
        }

        public OtherInfoPieces(BitString uPartyId, BitString dkmNonce)
        {
            UPartyId = uPartyId;
            DkmNonce = dkmNonce;
        }

        public BitString UPartyId { get; }
        public BitString DkmNonce { get; }
    }

    [TestFixture, FastCryptoTest]
    public class OtherInfoProofOfConceptTests
    {
        OtherInfoProofOfConcept _subject = new OtherInfoProofOfConcept();

        private static object[] _testData = new object[]
        {
            new object[]
            {
                new OtherInfoPieces(new BitString("CAFECAFE")), 
                "literal[12345abc]||uPartyId",
                new BitString("12345abccafecafe")
            },
            new object[]
            {
                new OtherInfoPieces(new BitString("CAFECAFE")),
                "uPartyId||literal[12345abc]",
                new BitString("cafecafe12345abc")
            },
            new object[]
            {
                new OtherInfoPieces(new BitString("CAFECAFE"), new BitString("faceface")),
                "literal[12345abc]||uPartyId",
                new BitString("12345abccafecafefaceface")
            },
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void TestStuff(OtherInfoPieces otherInfoPieces, string testPattern, BitString expectedBitString)
        {
            var result = _subject.DoStuff(otherInfoPieces, testPattern);

            Assert.AreEqual(expectedBitString.ToHex(), result.ToHex());
        }
    }
}
