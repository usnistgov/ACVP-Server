using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Describes retrieving a <see cref="IKdfOneStep"/> instance
    /// </summary>
    public interface IKdfOneStepFactory
    {
        /// <summary>
        /// Returns an instance of <see cref="IKdfOneStep"/> based on the specified parameters.
        /// </summary>
        /// <param name="kdfHashMode">The type of KDF</param>
        /// <param name="hashFunction">The hash function options</param>
        /// <returns></returns>
        [Obsolete]
        IKdfOneStep GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction);

        /// <summary>
        /// Returns an instance of <see cref="IKdfOneStep"/> based on the specified parameters.
        /// </summary>
        /// <param name="config">The KDF configuration.</param>
        /// <returns></returns>
        IKdfOneStep GetInstance(OneStepConfiguration config);
    }
}