using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function F() from FIPS 205 section 10.3. 
/// F(PK.seed,ADRS,M) = Truncn(SHA-256(PK.seed ∥ toByte(0,64−n) ∥ ADRSc ∥ M)) 
/// </summary>
public class FSha2SecurityCategories3and5 : IFHOrT
{
    private ISha _sha2_256;
    private int _n;
    
    public FSha2SecurityCategories3and5(int n, IShaFactory shaFactory)
    {
        _n = n;
        _sha2_256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
    }    
    
    /// <summary>
    /// Performs F(PK.seed,ADRS,M) = Truncn(SHA-256(PK.seed ∥ toByte(0,64−n) ∥ ADRSc ∥ M)) 
    /// </summary>
    /// <param name="pkSeed">PK.seed</param>
    /// <param name="adrs">ADRS</param>
    /// <param name="M">M</param>
    /// <returns></returns>
    public byte[] Hash(byte[] pkSeed, IAdrs adrs, byte[] M)
    {
        var zeros = SlhdsaHelpers.ToByte(0, 64 - _n);
        var compressedAdrsBytes = adrs.GetCompressedAdrs();
        
        // Build the byte string to be hashed, i.e., PK.seed ∥ toByte(0,64−n) ∥ ADRSc ∥ M
        var message = new byte[pkSeed.Length + zeros.Length + compressedAdrsBytes.Length + M.Length];
        Array.Copy(pkSeed, 0, message, 0, pkSeed.Length);
        Array.Copy(zeros, 0, message, pkSeed.Length, zeros.Length);
        Array.Copy(compressedAdrsBytes, 0, message, pkSeed.Length + zeros.Length, compressedAdrsBytes.Length);
        Array.Copy(M, 0, message, pkSeed.Length + zeros.Length + compressedAdrsBytes.Length, M.Length);
        
        var hashResult = _sha2_256.HashMessage(new BitString(message));
        var digest = hashResult.Digest.ToBytes();
        
        return digest[.._n];
    }
    
}
