using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class MonteCarloKeyMaker : IMonteCarloKeyMaker
    {
        public BitString MixKeys(TDESKeys keys, List<BitString> lastThreeCipherTexts)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (lastThreeCipherTexts== null || lastThreeCipherTexts.Count < 3)
            {
                throw new ArgumentException("Need three cipherText entries to mix keys", nameof(lastThreeCipherTexts));
            }

            var newKey1 = keys.KeysAsBitStrings[0].XOR(lastThreeCipherTexts[0]);
            var newKey2 = keys.KeysAsBitStrings[1].XOR(lastThreeCipherTexts[0]);
            var newKey3 = keys.KeysAsBitStrings[2].XOR(lastThreeCipherTexts[0]);
           
            if (keys.KeyOption == KeyOptionValues.ThreeKey)
            {
                newKey2 = keys.KeysAsBitStrings[1].XOR(lastThreeCipherTexts[1]);
                newKey3 = keys.KeysAsBitStrings[2].XOR(lastThreeCipherTexts[2]);
            }
            if (keys.KeyOption == KeyOptionValues.TwoKey)
            {
                newKey2 = keys.KeysAsBitStrings[1].XOR(lastThreeCipherTexts[1]);
            }
            byte[] outputArray = new byte[24];

            Array.Copy(newKey1.ToBytes(), outputArray, 8);
            Array.Copy(newKey2.ToBytes(), 0, outputArray ,8, 8);
            Array.Copy(newKey3.ToBytes(), 0, outputArray, 16, 8);
            return new BitString(outputArray);
        }
    }
}
