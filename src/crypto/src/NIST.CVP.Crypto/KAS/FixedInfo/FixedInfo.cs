using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.FixedInfo
{
    public class FixedInfo : IFixedInfo
    {
        private readonly IFixedInfoStrategyFactory _fixedInfoStrategyFactory;

        public FixedInfo(IFixedInfoStrategyFactory fixedInfoStrategyFactory)
        {
            _fixedInfoStrategyFactory = fixedInfoStrategyFactory;
        }
        
        public BitString Get(FixedInfoParameter param)
        {
            var fixedInfoParts = new Dictionary<string, BitString>();

            // split the pattern into pieces
            var pieces = param.FixedInfoPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (!pieces.Any())
            {
                return new BitString(0);
            }

            foreach (var piece in pieces)
            {
                var workingPiece = piece.Replace("||", "");
                GetDataFromPiece(fixedInfoParts, workingPiece, param);
            }
            
            return _fixedInfoStrategyFactory.Get(param.Encoding).GetFixedInfo(fixedInfoParts);
        }

        private void GetDataFromPiece(Dictionary<string, BitString> fixedInfoParts, string workingPiece, FixedInfoParameter fixedInfoParameter)
        {
            if (workingPiece.Equals("uPartyInfo", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyU.PartyId)
                    .ConcatenateBits(
                        BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyU.EphemeralData)));
                return;
            }

            if (workingPiece.Equals("vPartyInfo", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyV.PartyId)
                    .ConcatenateBits(
                        BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyV.EphemeralData)));
                return;
            }

            if (workingPiece.Equals("L", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.To32BitString(fixedInfoParameter.L));
                return;
            }

            if (workingPiece.Equals("salt", StringComparison.OrdinalIgnoreCase) && fixedInfoParameter.Salt?.BitLength > 0)
            {
                fixedInfoParts.Add(workingPiece, fixedInfoParameter.Salt);
                return;
            }
            
            if (workingPiece.Equals("iv", StringComparison.OrdinalIgnoreCase) && fixedInfoParameter.Iv?.BitLength > 0)
            {
                fixedInfoParts.Add(workingPiece, fixedInfoParameter.Iv);
                return;
            }
            
            if (workingPiece.Equals("counter", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, new BitString(32).BitStringAddition(BitString.One()));
                return;
            }

            if (workingPiece.StartsWith("literal[", StringComparison.OrdinalIgnoreCase))
            {
                // remove the "literal[]" to get just the hex value
                workingPiece = workingPiece.Replace("literal[", "").Replace("]", "");
                fixedInfoParts.Add(workingPiece, new BitString(workingPiece));
                return;
            }

            throw new ArgumentException(nameof(workingPiece));
        }
    }
}