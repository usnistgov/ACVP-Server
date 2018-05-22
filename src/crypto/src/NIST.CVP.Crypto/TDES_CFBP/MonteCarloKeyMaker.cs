using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class MonteCarloKeyMaker : IMonteCarloKeyMaker
    {
        public BitString MixKeys(TDESKeys keys, List<BitString> previousOutputs)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (previousOutputs == null || 
                (previousOutputs.Count * previousOutputs[0].BitLength) < 192)
            {
                throw new ArgumentException("Need 192 bits of previous outputs", nameof(previousOutputs));
            }
            var outputBitString = new BitString(192);
            previousOutputs.Reverse();
            foreach (var previousOutput in previousOutputs)
            {
                outputBitString = outputBitString.ConcatenateBits(previousOutput);
            }
            var newKey1 = keys.KeysAsBitStrings[0].XOR(outputBitString.Substring(0, 64));
            var newKey2 = keys.KeysAsBitStrings[1].XOR(outputBitString.Substring(0, 64));
            var newKey3 = keys.KeysAsBitStrings[2].XOR(outputBitString.Substring(0, 64));

            if (keys.KeyOption == KeyOptionValues.ThreeKey)
            {
                newKey2 = keys.KeysAsBitStrings[1].XOR(outputBitString.Substring(64, 64));
                newKey3 = keys.KeysAsBitStrings[2].XOR(outputBitString.Substring(128, 64));
            }
            if (keys.KeyOption == KeyOptionValues.TwoKey)
            {
                newKey2 = keys.KeysAsBitStrings[1].XOR(outputBitString.Substring(64, 64));
            }
            byte[] outputArray = new byte[24];

            Array.Copy(newKey1.ToBytes(), outputArray, 8);
            Array.Copy(newKey2.ToBytes(), 0, outputArray, 8, 8);
            Array.Copy(newKey3.ToBytes(), 0, outputArray, 16, 8);
            return new BitString(outputArray);
        }
    }
}