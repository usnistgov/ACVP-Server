using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common
{
    /// <summary>
    /// Builds a message randomizer as defined in https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-106.pdf.
    /// </summary>
    public interface IPreSigVerMessageRandomizerBuilder
    {
        /// <summary>
        /// Provides an entropy provider to the constructed instance.
        /// </summary>
        /// <param name="entropyProvider">The entropy provider to utilize in the message random.</param>
        /// <returns>the builder.</returns>
        IPreSigVerMessageRandomizerBuilder WithEntropyProvider(IEntropyProvider entropyProvider);
        /// <summary>
        /// Builds the message randomizer.
        /// </summary>
        /// <returns></returns>
        IPreSigVerMessageRandomizer Build();
    }
}