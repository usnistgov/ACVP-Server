using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.TDES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TDES_ECB
{
    public class MonteCarloKeyMaker : IMonteCarloKeyMakerTdes
    {
        public BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (lastThreeOpResults == null || lastThreeOpResults.Count < 3)
            {
                throw new ArgumentException("Need three cipherText entries to mix keys", nameof(lastThreeOpResults));
            }

            var newKey1 = keys.KeysAsBitStrings[0].XOR(lastThreeOpResults[0]);
            var newKey2 = keys.KeysAsBitStrings[1].XOR(lastThreeOpResults[0]);
            var newKey3 = keys.KeysAsBitStrings[2].XOR(lastThreeOpResults[0]);

            if (keys.KeyOption == KeyOptionValues.ThreeKey)
            {
                newKey2 = keys.KeysAsBitStrings[1].XOR(lastThreeOpResults[1]);
                newKey3 = keys.KeysAsBitStrings[2].XOR(lastThreeOpResults[2]);
            }
            if (keys.KeyOption == KeyOptionValues.TwoKey)
            {
                newKey2 = keys.KeysAsBitStrings[1].XOR(lastThreeOpResults[1]);
            }
            byte[] outputArray = new byte[24];

            Array.Copy(newKey1.ToBytes(), outputArray, 8);
            Array.Copy(newKey2.ToBytes(), 0, outputArray, 8, 8);
            Array.Copy(newKey3.ToBytes(), 0, outputArray, 16, 8);
            return new BitString(outputArray);
        }
    }
}
