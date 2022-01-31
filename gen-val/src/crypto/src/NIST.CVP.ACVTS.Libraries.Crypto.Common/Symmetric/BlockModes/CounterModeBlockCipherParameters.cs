using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes
{
    public class CounterModeBlockCipherParameters : ModeBlockCipherParameters, ICounterModeBlockCipherParameters
    {
        public BitString Result { get; set; }

        public CounterModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString iv,
            BitString key,
            BitString payload,
            BitString result,
            bool useInverseCipherMode = false)
        : base(direction, iv, key, payload, useInverseCipherMode)
        {
            Result = result;
        }

        public CounterModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString key,
            BitString payload,
            BitString result,
            bool useInverseCipherMode = false)
        : base(direction, key, payload, useInverseCipherMode)
        {
            Result = result;
        }
    }
}
