using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class OtherInfo : IOtherInfo
    {
        public const string _CAVS_OTHER_INFO_PATTERN = "uPartyInfo||vPartyInfo";

        private readonly BitString _otherInfo = new BitString(0);

        private readonly KeyAgreementRole _thisPartyKeyAgreementRole;
        private readonly FfcSharedInformation _thisPartySharedInformation;
        private readonly FfcSharedInformation _otherPartySharedInformation;

        public OtherInfo(IEntropyProvider entropyProvider, string otherInfoPattern, int otherInfoLength, KeyAgreementRole thisPartyKeyAgreementRole, FfcSharedInformation thisPartySharedInformation, FfcSharedInformation otherPartySharedInformation)
        {
            _thisPartyKeyAgreementRole = thisPartyKeyAgreementRole;
            _thisPartySharedInformation = thisPartySharedInformation;
            _otherPartySharedInformation = otherPartySharedInformation;

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
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartySharedInformation.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartySharedInformation.DkmNonce));
                }
                else
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartySharedInformation.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartySharedInformation.DkmNonce));
                }
                
                return oi;
            }

            if (workingPiece.Equals("vPartyInfo", StringComparison.OrdinalIgnoreCase))
            {
                if (_thisPartyKeyAgreementRole == KeyAgreementRole.ResponderPartyV)
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartySharedInformation.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_thisPartySharedInformation.DkmNonce));
                }
                else
                {
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartySharedInformation.PartyId));
                    oi = oi.ConcatenateBits(BitString.GetAtLeastZeroLengthBitString(_otherPartySharedInformation.DkmNonce));
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
