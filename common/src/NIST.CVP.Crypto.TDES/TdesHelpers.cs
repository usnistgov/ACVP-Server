using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.TDES
{
    /// <summary>
    /// Helper methods for TDES.
    /// </summary>
    public static class TdesHelpers
    {
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
    }
}
