using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Provides an implementation of the PRF function described in sections 10.1 - 10.3 of FIPS 205
/// </summary>
public interface IPrf
{
    /// <summary>
    /// Perform PRF() on the provided values. 
    /// </summary>
    byte[] GetPseudorandomByteString(byte[] pkSeed, byte[] skSeed, IAdrs adrs);  
}
