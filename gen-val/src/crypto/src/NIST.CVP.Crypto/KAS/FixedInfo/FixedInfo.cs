using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (string.IsNullOrEmpty(param?.FixedInfoPattern))
            {
                return new BitString(0);
            }

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
            
            if (workingPiece.Equals("uPartyId", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyU.PartyId));
                return;
            }
            
            if (workingPiece.Equals("uEphemeralData", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyU.EphemeralData));
                return;
            }

            if (workingPiece.Equals("vPartyInfo", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyV.PartyId)
                    .ConcatenateBits(
                        BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyV.EphemeralData)));
                return;
            }
            
            if (workingPiece.Equals("vPartyId", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyV.PartyId));
                return;
            }
            
            if (workingPiece.Equals("vEphemeralData", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.GetAtLeastZeroLengthBitString(fixedInfoParameter.FixedInfoPartyV.EphemeralData));
                return;
            }

            if (workingPiece.Equals("L", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, BitString.To32BitString(fixedInfoParameter.L));
                return;
            }

            if (workingPiece.Equals("salt", StringComparison.OrdinalIgnoreCase))
            {
                if (fixedInfoParameter.Salt?.BitLength > 0)
                {
                    // We only want to add the salt to the fixed info when it isn't the default
                    if (fixedInfoParameter.Salt.ToPositiveBigInteger() == 0)
                    {
                        return;
                    }
                    fixedInfoParts.Add(workingPiece, fixedInfoParameter.Salt);
                }
                return;
            }

            if (workingPiece.Equals("iv", StringComparison.OrdinalIgnoreCase))
            {
                if (fixedInfoParameter.Iv?.BitLength > 0)
                {
                    fixedInfoParts.Add(workingPiece, fixedInfoParameter.Iv);
                }
                return;
            }

            if (workingPiece.Equals("counter", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, new BitString(32).BitStringAddition(BitString.One()));
                return;
            }

            if (workingPiece.Equals("algorithmId", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, fixedInfoParameter.AlgorithmId);
                return;
            }

            if (workingPiece.Equals("label", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, fixedInfoParameter.Label);
                return;
            }

            if (workingPiece.Equals("context", StringComparison.OrdinalIgnoreCase))
            {
                fixedInfoParts.Add(workingPiece, fixedInfoParameter.Context);
                return;
            }

            if (workingPiece.StartsWith("literal[", StringComparison.OrdinalIgnoreCase))
            {
                // remove the "literal[]" to get just the hex value
                workingPiece = workingPiece.Replace("literal[", "").Replace("]", "");
                fixedInfoParts.Add(workingPiece, new BitString(workingPiece));
                return;
            }

            throw new ArgumentException($"{nameof(workingPiece)}: {workingPiece}");
        }
    }
}