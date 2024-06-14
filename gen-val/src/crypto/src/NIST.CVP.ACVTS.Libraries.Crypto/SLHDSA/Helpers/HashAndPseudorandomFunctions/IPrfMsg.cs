namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Provides an implementation of the PRFmsg function described in sections 10.1 - 10.3 of FIPS 205
/// </summary>
public interface IPrfMsg
{
    /// <summary>
    /// Perform PRFmsg() on the provided values. 
    /// </summary>
    byte[] GetPseudorandomByteString(byte[] skPrf, byte[] optRand, byte[] M);
}
