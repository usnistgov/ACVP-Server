using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF
{
    public class PipelineKdf : KdfBase, IKdf
    {
        private readonly CounterLocations _counterLocation;
        private readonly int _counterLength;

        public PipelineKdf(IMac mac, CounterLocations counterLocation, int counterLength) : base(mac)
        {
            _counterLocation = counterLocation;
            _counterLength = counterLength;
        }

        public KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv = null, int breakLocation = 0)
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

            var prevA = fixedData.GetDeepCopy();
            for (var i = 1; i <= n; i++)
            {
                var aTemp = PseudoRandomFunction(kI, prevA);
                var counterBits = BitString.To32BitString(i).GetLeastSignificantBits(_counterLength);
                var data = new BitString(0);

                switch (_counterLocation)
                {
                    case CounterLocations.BeforeIterator:
                        // counter || aTemp || fixedData
                        data = data.ConcatenateBits(counterBits).ConcatenateBits(aTemp).ConcatenateBits(fixedData);
                        break;

                    case CounterLocations.AfterFixedData:
                        // aTemp || fixedData || counter
                        data = data.ConcatenateBits(aTemp).ConcatenateBits(fixedData).ConcatenateBits(counterBits);
                        break;

                    case CounterLocations.BeforeFixedData:
                    case CounterLocations.MiddleFixedData:
                        // aTemp || counter || fixedData
                        data = data.ConcatenateBits(aTemp).ConcatenateBits(counterBits).ConcatenateBits(fixedData);
                        break;

                    case CounterLocations.None:
                        // aTemp || fixedData
                        data = data.ConcatenateBits(aTemp).ConcatenateBits(fixedData);
                        break;

                    default:
                        return new KdfResult("Invalid Counter location");
                }

                prevA = aTemp.GetDeepCopy();
                var kTemp = PseudoRandomFunction(kI, data);
                result = result.ConcatenateBits(kTemp);
            }

            var kOut = result.GetMostSignificantBits(len);
            return new KdfResult(kOut);
        }
    }
}
