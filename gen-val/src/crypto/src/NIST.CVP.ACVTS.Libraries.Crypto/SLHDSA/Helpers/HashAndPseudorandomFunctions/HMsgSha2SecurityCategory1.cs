using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.KTS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function Hmsg(R,PK.seed,PK.root,M)=MGF1-SHA-256(R∥PK.seed∥SHA-256(R∥PK.seed∥PK.root∥M),m)
/// from FIPS 205 section 10.2.
/// </summary>
public class HMsgSha2SecurityCategory1 : IHMsg
{
    private ISha _sha2_256;

    // The Hmsg function is a function of parameters m and M, among others. _m is m (vs M). Values for m are provided in 
    // FIPS 205 Section 10 Table 1.
    private int _m;
    private Mgf _mgf1Sha256;

    public HMsgSha2SecurityCategory1(int m, IShaFactory shaFactory)
    {
        _m = m;
        _sha2_256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
        _mgf1Sha256 = new Mgf(_sha2_256);
    }

    /// <summary>
    ///  Performs Hmsg(R,PK.seed,PK.root,M)=MGF1-SHA-256(R∥PK.seed∥SHA-256(R∥PK.seed∥PK.root∥M),m)
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
        // SHA-256(R ∥ PK.seed ∥ PK.root ∥ M)
        var hashResult = _sha2_256.HashMessage(new BitString(message));
        var hashResultBytes = hashResult.Digest.ToBytes();

        // Build the byte string for the mask generation function, MGF1, i.e., R ∥ PK.seed ∥ SHA-256(R ∥ PK.seed ∥ PK.root ∥ M)
        var mgf1Seed = new byte[r.Length + pkSeed.Length + hashResultBytes.Length];
        Array.Copy(r, 0, mgf1Seed, 0, r.Length);
        Array.Copy(pkSeed, 0, mgf1Seed, r.Length, pkSeed.Length);
        Array.Copy(hashResultBytes, 0, mgf1Seed, r.Length + pkSeed.Length, hashResultBytes.Length);
        // MGF1-SHA-256(R ∥ PK.seed ∥ SHA-256(R ∥ PK.seed ∥ PK.root ∥ M),m)
        var mgf1Result = _mgf1Sha256.Generate(new BitString(mgf1Seed), _m * 8);

        return mgf1Result.ToBytes();
    }
}
