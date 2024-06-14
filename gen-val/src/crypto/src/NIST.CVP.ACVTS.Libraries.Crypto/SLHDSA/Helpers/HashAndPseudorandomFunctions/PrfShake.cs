using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function Prf() from FIPS 205 section 10.1.
/// PRF(PK.seed, SK.seed, ADRS) = SHAKE256(PK.seed ∥ ADRS ∥ SK.seed, 8n)  
/// </summary>
public class PrfShake : IPrf
{
    private ISha _shake256;
    private int _n;
    
    public PrfShake(int n, IShaFactory shaFactory)
    {
        _n = n;
        _shake256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
    }
    
    /// <summary>
    /// Performs PRF(PK.seed, SK.seed, ADRS) = SHAKE256(PK.seed ∥ ADRS ∥ SK.seed, 8n).
    /// </summary>
    /// <param name="pkSeed">PK.seed</param>
    /// <param name="skSeed">SK.seed</param>
    /// <param name="adrs">ADRS</param>
    /// <returns></returns>
    public byte[] GetPseudorandomByteString(byte[] pkSeed, byte[] skSeed, IAdrs adrs)
    {
        var adrsBytes = adrs.GetAdrs();
        // Build the byte string to be hashed, i.e., PK.seed ∥ ADRS ∥ SK.seed, 8n
        var message = new byte[pkSeed.Length + adrsBytes.Length + skSeed.Length];
        Array.Copy(pkSeed, 0, message, 0, pkSeed.Length);
        Array.Copy(adrsBytes, 0, message, pkSeed.Length, adrsBytes.Length);
        Array.Copy(skSeed, 0, message, pkSeed.Length + adrsBytes.Length, skSeed.Length);
        var result = _shake256.HashMessage(new BitString(message), 8*_n);

        return result.Digest.ToBytes();
    }
}
