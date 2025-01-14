using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Provides an implementation of the F, H or Tl function described in sections 10.1 - 10.3 of FIPS 205. Since F, H,
/// and Tl are almost completely identical to each other in sections 10.1, 10.2, and 10.3, we use one interface for the
/// three functions, i.e., IFHOrT. 
/// </summary>
public interface IFHOrT
{
    /// <summary>
    /// Perform F(), H() or Tl() on the provided values. 
    /// </summary>
    byte[] Hash(byte[] pkSeed, IAdrs adrs, byte[] M);
}
