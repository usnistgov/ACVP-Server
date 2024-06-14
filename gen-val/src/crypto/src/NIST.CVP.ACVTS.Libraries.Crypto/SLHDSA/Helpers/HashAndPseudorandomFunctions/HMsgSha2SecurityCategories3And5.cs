using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.KTS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function Hmsg(R,PK.seed,PK.root,M) = MGF1-SHA-512(R ∥ PK.seed ∥ SHA-512(R ∥ PK.seed ∥ PK.root ∥ M),m)
/// from FIPS 205 section 10.3.
/// </summary>
public class HMsgSha2SecurityCategories3And5 : IHMsg
{
    private ISha _sha2_512;

    // The Hmsg function is a function of parameters m and M, among others. _m is m (vs M). Values for m are provided in 
    // FIPS 205 Section 10 Table 1.
    private int _m;
    private Mgf _mgf1Sha512;

    public HMsgSha2SecurityCategories3And5(int m, IShaFactory shaFactory)
    {
        _m = m;
        _sha2_512 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));
        _mgf1Sha512 = new Mgf(_sha2_512);
    }
    
    /// <summary>
    ///  Hmsg(R,PK.seed,PK.root,M) = MGF1-SHA-512(R ∥ PK.seed ∥ SHA-512(R ∥ PK.seed ∥ PK.root ∥ M),m)
    /// </summary>
    /// <param name="r">R</param>
    /// <param name="pkSeed">PK.seed</param>
    /// <param name="pkRoot">PK.root</param>
    /// <param name="M">M</param>
    /// <returns></returns>
    public byte[] Hash(byte[] r, byte[] pkSeed, byte[] pkRoot, byte[] M)
    {
        // Build the byte string to be hashed, i.e., R ∥ PK.seed ∥ PK.root ∥ M
        var message = new byte[r.Length + pkSeed.Length + pkRoot.Length + M.Length];
        Array.Copy(r, 0, message, 0, r.Length);
        Array.Copy(pkSeed, 0, message, r.Length, pkSeed.Length);
        Array.Copy(pkRoot, 0, message, r.Length + pkSeed.Length, pkRoot.Length);
        Array.Copy(M, 0, message, r.Length + pkSeed.Length + pkRoot.Length, M.Length);
        // SHA-512(R ∥ PK.seed ∥ PK.root ∥ M)
        var hashResult = _sha2_512.HashMessage(new BitString(message));
        var hashResultBytes = hashResult.Digest.ToBytes();

        // Build the byte string for the mask generation function, MGF1, i.e., R ∥ PK.seed ∥ SHA-512(R ∥ PK.seed ∥ PK.root ∥ M)
        var mgf1Seed = new byte[r.Length + pkSeed.Length + hashResultBytes.Length];
        Array.Copy(r, 0, mgf1Seed, 0, r.Length);
        Array.Copy(pkSeed, 0, mgf1Seed, r.Length, pkSeed.Length);
        Array.Copy(hashResultBytes, 0, mgf1Seed, r.Length + pkSeed.Length, hashResultBytes.Length);
        // MGF1-SHA-512(R ∥ PK.seed ∥ SHA-512(R ∥ PK.seed ∥ PK.root ∥ M),m)
        var mgf1Result = _mgf1Sha512.Generate(new BitString(mgf1Seed), _m * 8);

        return mgf1Result.ToBytes();
    }
}
