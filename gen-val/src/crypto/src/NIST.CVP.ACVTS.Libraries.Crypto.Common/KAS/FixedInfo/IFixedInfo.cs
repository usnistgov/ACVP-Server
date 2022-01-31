using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo
{
    /// <summary>
    /// Interface for building a fixed info string.
    /// </summary>
    public interface IFixedInfo
    {
        /// <summary>
        /// Builds a Fixed Info string based on the provided <see cref="FixedInfoParameter"/>.
        /// </summary>
        /// <param name="param">The parameters that make up the Fixed Info string.</param>
        /// <returns>The constructed fixed info string.</returns>
        BitString Get(FixedInfoParameter param);
    }
}
