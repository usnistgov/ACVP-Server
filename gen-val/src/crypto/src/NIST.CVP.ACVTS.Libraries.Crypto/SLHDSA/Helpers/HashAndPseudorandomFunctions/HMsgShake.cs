using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function Hmsg(R,PK.seed,PK.root,M) = SHAKE256(R ∥ PK.seed ∥ PK.root ∥ M,8m) from FIPS 205 section 10.1.
/// </summary>
public class HMsgShake: IHMsg
{
    private ISha _shake256;
    // The Hmsg function is a function of parameters m and M, among others. _m is m (vs M). Values for m are provided in 
    // FIPS 205 Section 10 Table 1.
    private int _m;

    public HMsgShake(int m, IShaFactory shaFactory)
    {
        _m = m;
        _shake256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
    }
    
    /// <summary>
    ///  Performs Hmsg(R,PK.seed,PK.root,M) = SHAKE256(R ∥ PK.seed ∥ PK.root ∥ M,8m)
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
        var result = _shake256.HashMessage(new BitString(message), 8*_m);

        return result.Digest.ToBytes();
    }
}
