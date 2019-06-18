using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx
{
    public static class FfxRadixHelper
    {
        private const int MaxFfxPayloadLength = 1000000;
        
        /// <summary>
        /// Determines the validity of the radix, minPayload, maxPayload combination.
        /// </summary>
        /// <param name="radix">The radix/base.</param>
        /// <param name="minPayload">The minimum payload allowed for encryption with the provided radix.</param>
        /// <param name="maxPayload">The maximum payload allowed for encryption with the provided radix.</param>
        /// <returns></returns>
        public static bool IsRadixValidWithPayload(int radix, int minPayload, int maxPayload)
        {
            // radix ∈ [2..Pow(2, 16) ],
            if (radix < 2 || radix > (int) System.Math.Pow(2, 16))
            {
                return false;
            }
            
            // Pow(radix, minlen) ≥ 1 000 000, and
            if (System.Math.Pow(radix, minPayload) < MaxFfxPayloadLength)
            {
                return false;
            }
            
            // 2 ≤ minlen ≤ maxlen ≤ 2*Floor(logradix (Pow(2, 96) )).
            return 2 <= minPayload && minPayload <= maxPayload && maxPayload <=
                   2 * (int) System.Math.Floor(System.Math.Log(System.Math.Pow(2, 96), radix));
        }
    }
}