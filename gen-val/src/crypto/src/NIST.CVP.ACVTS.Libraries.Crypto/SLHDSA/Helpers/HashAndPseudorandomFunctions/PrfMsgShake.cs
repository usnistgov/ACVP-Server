using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function PRFmsg(SK.prf,opt_rand,M) = SHAKE256(SK.prf ∥ opt_rand ∥ M,8n) from FIPS 205 section 10.1.
/// </summary>
public class PrfMsgShake : IPrfMsg
{
    private ISha _shake256;
    private int _n;

    public PrfMsgShake(int n, IShaFactory shaFactory)
    {
        _n = n;
        _shake256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
    }
    
    /// <summary>
    /// Performs PRFmsg(SK.prf,opt_rand,M) = SHAKE256(SK.prf ∥ opt_rand ∥ M,8n)
    /// </summary>
    /// <param name="skPrf">SK.prf</param>
    /// <param name="optRand">opt_rand</param>
    /// <param name="M">M</param>
    /// <returns></returns>
    public byte[] GetPseudorandomByteString(byte[] skPrf, byte[] optRand, byte[] M)
    {
        // Build the byte string to be hashed/(shaken, ha ha ha :), i.e., SK.prf ∥ opt_rand ∥ M
        var message = new byte[skPrf.Length + optRand.Length + M.Length];
        Array.Copy(skPrf, 0, message, 0, skPrf.Length);
        Array.Copy(optRand, 0, message, skPrf.Length, optRand.Length);
        Array.Copy(M, 0, message, skPrf.Length + optRand.Length, M.Length);
        var result = _shake256.HashMessage(new BitString(message), 8*_n);

        return result.Digest.ToBytes();
    }
}
