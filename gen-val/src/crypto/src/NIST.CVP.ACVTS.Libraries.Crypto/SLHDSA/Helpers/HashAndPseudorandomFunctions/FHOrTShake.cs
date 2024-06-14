using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the functions F(), H(), and Tl() from FIPS 205 section 10.1. Slightly generalized,
/// F(PK.seed,ADRS,M) = H(PK.seed,ADRS,M) = Tl(PK.seed,ADRS,M) = SHAKE256(PK.seed ∥ ADRS ∥ M, 8n)  
/// </summary>
public class FHOrTShake : IFHOrT
{
    private ISha _shake256;
    private int _n;
    
    public FHOrTShake(int n, IShaFactory shaFactory)
    {
        _n = n;
        _shake256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
    }
    
    /// <summary>
    /// Performs F(PK.seed,ADRS,M) = H(PK.seed,ADRS,M) = Tl(PK.seed,ADRS,M) = SHAKE256(PK.seed ∥ ADRS ∥ M, 8n) 
    /// </summary>
    /// <param name="pkSeed">PK.seed</param>
    /// <param name="adrs">ADRS</param>
    /// <param name="M">M</param>
    /// <returns></returns>
    public byte[] Hash(byte[] pkSeed, IAdrs adrs, byte[] M)
    {
        var adrsBytes = adrs.GetAdrs();
        // Build the byte string to be hashed, i.e., PK.seed ∥ ADRS ∥ M
        var message = new byte[pkSeed.Length + adrsBytes.Length + M.Length];
        Array.Copy(pkSeed, 0, message, 0, pkSeed.Length);
        Array.Copy(adrsBytes, 0, message, pkSeed.Length, adrsBytes.Length);
        Array.Copy(M, 0, message, pkSeed.Length + adrsBytes.Length, M.Length);
        var result = _shake256.HashMessage(new BitString(message), 8*_n);

        return result.Digest.ToBytes();
    }
}
