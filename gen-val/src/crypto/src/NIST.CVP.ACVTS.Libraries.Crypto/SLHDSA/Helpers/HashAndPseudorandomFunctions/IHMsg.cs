namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Provides an implementation of the Hmsg function described in sections 10.1 - 10.3 of FIPS 205
/// </summary>
public interface IHMsg
{
    /// <summary>
    /// Perform Hmsg() on the provided values. 
    /// </summary>
    byte[] Hash(byte[] r, byte[] pkSeed, byte[] pkRoot, byte[] M);
}
