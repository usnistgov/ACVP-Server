using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS
{
    /// <summary>
    /// Factory for getting an instance of <see cref="IRsaOaep"/> with the specified hash function.
    /// </summary>
    public interface IKtsFactory
    {
        /// <summary>
        /// Get an instance of a <see cref="IRsaOaep"/> to wrap a key for use in KTS schemes of KAS
        /// </summary>
        /// <param name="hashAlg">The hash algorithm used for the wrapping algorithm.</param>
        /// <returns>An instance of <see cref="IRsaOaep"/></returns>
        IRsaOaep Get(KasHashAlg hashAlg);
    }
}
