using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo
{
    public class OtherInfo : IOtherInfo
    {
        public const string _CAVS_OTHER_INFO_PATTERN = "uPartyInfo||vPartyInfo";

        private readonly BitString _otherInfo = new BitString(0);

        private readonly KeyAgreementRole _thisPartyKeyAgreementRole;
        private readonly PartyOtherInfo _thisPartyOtherInfo;
        private readonly PartyOtherInfo _otherPartyOtherInfo;

        public OtherInfo(
            IEntropyProvider entropyProvider,
            string otherInfoPattern,
            int otherInfoLength,
            KeyAgreementRole thisPartyKeyAgreementRole,
            PartyOtherInfo thisPartyOtherInfo,
            PartyOtherInfo otherPartyOtherInfo
        )
        {
            _thisPartyKeyAgreementRole = thisPartyKeyAgreementRole;
            _thisPartyOtherInfo = thisPartyOtherInfo;
            _otherPartyOtherInfo = otherPartyOtherInfo;

            // split the pattern into pieces
            var pieces = otherInfoPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var piece in pieces)
            {
                var workingPiece = piece.Replace("||", "");
                _otherInfo = _otherInfo.ConcatenateBits(ConcatenatePieceOntoOtherInfo(workingPiece));
            }

            // Add entropy to hit otherInfoLength
            _otherInfo = _otherInfo.ConcatenateBits(entropyProvider.GetEntropy(otherInfoLength - _otherInfo.BitLength));

            _otherInfo = _otherInfo.GetMostSignificantBits(otherInfoLength);
        }

        public BitString GetOtherInfo()
        {
            return _otherInfo;
        }

        private BitString ConcatenatePieceOntoOtherInfo(string workingPiece)
        {
            BitString oi = new BitString(0);

            if (workingPiece.Equals("uPartyInfo", StringComparison.OrdinalIgnoreCase))
            {
                if (_thisPartyKeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartyOtherInfo.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartyOtherInfo.DkmNonce));
                }
                else
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartyOtherInfo.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartyOtherInfo.DkmNonce));
                }

                return oi;
            }

            if (workingPiece.Equals("vPartyInfo", StringComparison.OrdinalIgnoreCase))
            {
                if (_thisPartyKeyAgreementRole == KeyAgreementRole.ResponderPartyV)
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartyOtherInfo.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartyOtherInfo.DkmNonce));
                }
                else
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartyOtherInfo.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartyOtherInfo.DkmNonce));
                }

                return oi;
            }

            if (workingPiece.Equals("counter", StringComparison.OrdinalIgnoreCase))
            {
                oi = oi.ConcatenateBits(new BitString(32).BitStringAddition(BitString.One()));

                return oi;
            }

            if (workingPiece.StartsWith("literal[", StringComparison.OrdinalIgnoreCase))
            {
                // remove the "literal[]" to get just the hex value
                workingPiece = workingPiece.Replace("literal[", "").Replace("]", "");
                oi = oi.ConcatenateBits(new BitString(workingPiece));

                return oi;
            }

            throw new ArgumentException(nameof(workingPiece));
        }
    }
}
