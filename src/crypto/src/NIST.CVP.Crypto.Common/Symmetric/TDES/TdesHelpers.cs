using System;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    /// <summary>
    /// Helper methods for TDES.
    /// </summary>
    public static class TdesHelpers
    {
        private static Random800_90 random800_90 = new Random800_90();

        /// <summary>
        /// Gets the number of keys from the keying option.
        /// Keying option 1: key1, key2, key3
        /// Keying option 2: key1, key2, key1
        /// Keying option 3: key1, key1, key1
        /// </summary>
        /// <param name="keyingOption">The keying option to get the number of keys for</param>
        /// <returns></returns>
        public static int GetNumberOfKeysFromKeyingOption(int keyingOption)
        {
            switch (keyingOption)
            {
                case 1:
                    return 3;
                case 2:
                    return 2;
                case 3:
                    return 1;
                default:
                    throw new ArgumentException(nameof(keyingOption));
            }
        }


        /// <summary>
        /// Gets the number of keys from the keying option.
        /// </summary>
        /// <param name="numberOfKeys">The the number of keys to get keying option for</param>
        /// <returns></returns>
        public static int GetKeyingOptionFromNumberOfKeys(int numberOfKeys)
        {
            switch (numberOfKeys)
            {
                case 1:
                    return 3;
                case 2:
                    return 2;
                case 3:
                    return 1;
                default:
                    throw new ArgumentException(nameof(numberOfKeys));
            }
        }

        /// <summary>
        /// Generates a random key based on the key option
        /// Keying option 1: key1, key2, key3
        /// Keying option 2: key1, key2, key1
        /// </summary>
        /// <param name="keyingOption">The keying option to get the number of keys for</param>
        /// <returns></returns>
        public static BitString GenerateTdesKey(int keyingOption)
        {
            if (keyingOption == 1)
            {
                var key1 = random800_90.GetRandomBitString(64);
                var key2 = random800_90.GetRandomBitString(64);
                var key3 = random800_90.GetRandomBitString(64);
                return key1.ConcatenateBits(key2).ConcatenateBits(key3);
            }
            else if (keyingOption == 2)
            {
                var key1 = random800_90.GetRandomBitString(64);
                var key2 = random800_90.GetRandomBitString(64);
                return key1.ConcatenateBits(key2).ConcatenateBits(key1);
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(keyingOption)} provided.");
            }
        }
    }
}
