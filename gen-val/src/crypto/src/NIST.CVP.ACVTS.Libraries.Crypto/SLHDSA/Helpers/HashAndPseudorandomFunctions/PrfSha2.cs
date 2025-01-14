using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function Prf() from FIPS 205 sections 10.2 and 10.3. The definitions of Prf() in sections 10.2 and
/// 10.3 are the same. PRF(PK.seed,SK.seed,ADRS) = Truncn(SHA-256(PK.seed ∥ toByte(0,64 − n) ∥ ADRSc ∥ SK.seed))
/// </summary>
public class PrfSha2 : IPrf
{
    private ISha _sha2_256;
    private int _n;
    
    public PrfSha2(int n, IShaFactory shaFactory)
    {
        _n = n;
        _sha2_256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
    }
    
    /// <summary>
    /// Performs PRF(PK.seed,SK.seed,ADRS) = Truncn(SHA-256(PK.seed ∥ toByte(0,64 − n) ∥ ADRSc ∥ SK.seed))
    /// </summary>
    /// <param name="pkSeed">PK.seed</param>
    /// <param name="skSeed">SK.seed</param>
    /// <param name="adrs">ADRS</param>
    /// <returns></returns>
    public byte[] GetPseudorandomByteString(byte[] pkSeed, byte[] skSeed, IAdrs adrs)
    {
        var zeros = SlhdsaHelpers.ToByte(0, 64 - _n);
        var compressedAdrsBytes = adrs.GetCompressedAdrs();
        
        // Build the byte string to be hashed, i.e., PK.seed ∥ toByte(0,64 − n) ∥ ADRSc ∥ SK.seed
        var message = new byte[pkSeed.Length + zeros.Length + compressedAdrsBytes.Length + skSeed.Length];
        Array.Copy(pkSeed, 0, message, 0, pkSeed.Length);
        Array.Copy(zeros, 0, message, pkSeed.Length, zeros.Length);
        Array.Copy(compressedAdrsBytes, 0, message, pkSeed.Length + zeros.Length, compressedAdrsBytes.Length);
        Array.Copy(skSeed, 0, message, pkSeed.Length + zeros.Length + compressedAdrsBytes.Length, skSeed.Length);
        
        var hashResult = _sha2_256.HashMessage(new BitString(message));
        var digest = hashResult.Digest.ToBytes();
        
        return digest[.._n];
    }
    
}
