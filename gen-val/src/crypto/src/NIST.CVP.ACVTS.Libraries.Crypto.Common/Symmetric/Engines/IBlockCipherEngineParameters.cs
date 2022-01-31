using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines
{
    /// <summary>
    /// Interface representing shared parameters for all symmetric ciphers
    /// </summary>
    public interface IBlockCipherEngineParameters : ICryptoParameters
    {
        /// <summary>
        /// The direction (encrypt or decrypt)
        /// </summary>
        BlockCipherDirections Direction { get; }
        /// <summary>
        /// The byte array representing the key (MSB)
        /// </summary>
        byte[] Key { get; }
        /// <summary>
        /// Should the inverse cipher mode be used?
        /// </summary>
        bool UseInverseCipherMode { get; }
    }
}
