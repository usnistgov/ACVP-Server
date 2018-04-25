using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Math
{
    /// <summary>
    /// Converts a <see cref="BigInteger"/> into Non-Adjacent Form so that in {-1, 0, 1} there are no consecutive nonzero values. This reduces the number
    /// of meaningful bits when applying multiplication over ECC.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>An array containing only {-1, 0, 1} values.</returns>
    public class NonAdjacentBitString
    {
        public int[] Bits { get { return _listBits.ToArray(); } }
        public int BitLength { get { return _listBits.Count; } }

        private List<int> _listBits;

        public NonAdjacentBitString(BigInteger value)
        {
            _listBits = new List<int>();
            var newValue = 0;
            var bits = new BitString(value).Bits;

            while (value > 0)
            {
                if (!value.IsEven)
                {
                    newValue = 2 - (int)(value % 4);
                    value -= newValue;
                }
                else
                {
                    newValue = 0;
                }

                _listBits.Add(newValue);
                value >>= 1;
            }
        }
    }
}
