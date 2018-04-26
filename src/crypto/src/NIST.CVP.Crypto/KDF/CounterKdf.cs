using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KDF
{
    public class CounterKdf : KdfBase, IKdf
    {
        private readonly CounterLocations _counterLocation;
        private readonly int _counterLength;

        public CounterKdf(IMac mac, CounterLocations counterLocation, int counterLength) : base(mac)
        {
            _counterLocation = counterLocation;
            _counterLength = counterLength;
        }

        public KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv = null, int breakLocation = 0)
        {
            // 1
            var n = len.CeilingDivide(Mac.OutputLength);

            // 2
            if (n > NumberTheory.Pow2(_counterLength) - 1)
            {
                return new KdfResult("Counter length too long for operation");
            }

            // 3
            var result = new BitString(0);

            // 4
            if (_counterLocation == CounterLocations.MiddleFixedData)
            {
                if (breakLocation < 1 || breakLocation > fixedData.BitLength - 1)
                {
                    return new KdfResult("Invalid break location");
                }
            }

            for (var i = 1; i <= n; i++)
            {
                var counterBits = BitString.To32BitString(i).GetLeastSignificantBits(_counterLength);
                var data = new BitString(0);

                switch (_counterLocation)
                {
                    case CounterLocations.BeforeFixedData:
                        data = data.ConcatenateBits(counterBits).ConcatenateBits(fixedData);
                        break;

                    case CounterLocations.AfterFixedData:
                        data = data.ConcatenateBits(fixedData).ConcatenateBits(counterBits);
                        break;

                    case CounterLocations.MiddleFixedData:
                        var firstPart = fixedData.GetMostSignificantBits(breakLocation);
                        var secondPart = fixedData.MSBSubstring(breakLocation, fixedData.BitLength - breakLocation);
                        data = data.ConcatenateBits(firstPart).ConcatenateBits(counterBits).ConcatenateBits(secondPart);
                        break;
                        
                    default:
                        return new KdfResult("Invalid Counter location");
                }

                var kTemp = PseudoRandomFunction(kI, data);

                result = result.ConcatenateBits(kTemp);
            }

            // 5
            var kOut = result.GetMostSignificantBits(len);
            return new KdfResult(kOut);
        }
    }
}
