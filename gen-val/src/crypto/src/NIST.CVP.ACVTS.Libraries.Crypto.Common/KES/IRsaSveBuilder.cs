using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KES
{
    /// <summary>
    /// Used for building an instance of <see cref="IRsaSve"/>.
    /// </summary>
    public interface IRsaSveBuilder
    {
        /// <summary>
        /// Provide an <see cref="IRsa"/> implementation to the builder.
        /// </summary>
        /// <param name="value">The <see cref="IRsa"/> implementation.</param>
        /// <returns>This builder.</returns>
        IRsaSveBuilder WithRsa(IRsa value);
        /// <summary>
        /// Provide an <see cref="IEntropyProvider"/> to the builder.
        /// </summary>
        /// <param name="value">The <see cref="IEntropyProvider"/> implementation.</param>
        /// <returns>This builder.</returns>
        IRsaSveBuilder WithEntropyProvider(IEntropyProvider value);
        /// <summary>
        /// Builds the <see cref="IRsaSve"/> based on the builder parameters.
        /// </summary>
        /// <returns>A constructed <see cref="IRsaSve"/>.</returns>
        IRsaSve Build();
    }
}
