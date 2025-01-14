using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

public class PrivateKey
{
    // All are n-bytes long
    public byte[] SkSeed { get; set; }
    public byte[] SkPrf { get; set; }
    public byte[] PkSeed { get; set; }
    public byte[] PkRoot { get; set; }

    public PrivateKey(byte[] skSeed, byte[] skPrf, byte[] pkSeed, byte[] pkRoot)
    {
        SkSeed = skSeed;
        SkPrf = skPrf;
        PkSeed = pkSeed;
        PkRoot = pkRoot;
    }

    public PrivateKey(byte[] privateKey)
    {
        // The full private key is the concatenation of all 4 values, which are equal length
        // skSeed || skPrf || pkSeed || pkRoot
        var n = privateKey.Length / 4;
        
        SkSeed = privateKey[..n];
        SkPrf  = privateKey[n..(2 * n)];
        PkSeed = privateKey[(2 * n)..(3 * n)];
        PkRoot = privateKey[(3 * n)..];
    }

    public byte[] GetBytes()
    {
        var privateKeyBytes = new byte[SkSeed.Length + SkPrf.Length + PkSeed.Length + PkRoot.Length];
        Array.Copy(SkSeed, 0, privateKeyBytes, 0, SkSeed.Length);
        Array.Copy(SkPrf, 0, privateKeyBytes, SkSeed.Length, SkPrf.Length);
        Array.Copy(PkSeed, 0, privateKeyBytes, SkSeed.Length + SkPrf.Length, PkSeed.Length);
        Array.Copy(PkRoot, 0, privateKeyBytes, SkSeed.Length + SkPrf.Length + PkSeed.Length, PkRoot.Length);
        
        return privateKeyBytes;
    }
}
