using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.MAC;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KDF
{
    public class FeedbackKdf : KdfBase, IKdf
    {
        private readonly CounterLocations _counterLocation;
        private readonly int _counterLength;

        public FeedbackKdf(IMac mac, CounterLocations counterLocation, int counterLength = 0) : base(mac)
        {
            _counterLocation = counterLocation;
            _counterLength = counterLength;
        }

        public KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv, int breakLocation = 0)
        {
            // 1
            var n = len.CeilingDivide(Mac.OutputLength);

            // 2
            if (n > NumberTheory.Pow2(32) - 1)
            {
                return new KdfResult("Counter length too long for operation");
            }

            // 3
            var result = new BitString(0);

            // 4
            if (_counterLocation == CounterLocations.None)
            {
                if (_counterLength != 0)
                {
                    return new KdfResult("Counter must have 0 length");
                }
            }

            var prevK = iv.GetDeepCopy();
            for (var i = 1; i <= n; i++)
            {
                var counterBits = BitString.To32BitString(i).GetLeastSignificantBits(_counterLength);
                var data = new BitString(0);

                switch (_counterLocation)
                {
                    case CounterLocations.BeforeFixedData:
                        // counter || prevK || fixedData
                        data = data.ConcatenateBits(counterBits).ConcatenateBits(prevK).ConcatenateBits(fixedData);
                        break;

                    case CounterLocations.AfterFixedData:
                        // prevK || fixedData || counter
                        data = data.ConcatenateBits(prevK).ConcatenateBits(fixedData).ConcatenateBits(counterBits);
                        break;

                    case CounterLocations.MiddleFixedData:
                        // prevK || counter || fixedData
                        data = data.ConcatenateBits(prevK).ConcatenateBits(counterBits).ConcatenateBits(fixedData);
                        break;
                       
                    case CounterLocations.None:
                        // prevK || fixedData
                        data = data.ConcatenateBits(prevK).ConcatenateBits(fixedData);
                        break;

                    default:
                        return new KdfResult("Invalid Counter location");
                }

                prevK = PseudoRandomFunction(kI, data);

                result = result.ConcatenateBits(prevK);
            }

            // 5
            var kOut = result.GetMostSignificantBits(len);
            return new KdfResult(kOut);
        }
    }
}
