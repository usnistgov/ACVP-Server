using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;

public class PrivateKey
{
    public byte[] SkSeed { get; set; }
    public byte[] SkPrf { get; set; }
    public byte[] PkSeed { get; set; }
    public byte[] PkRoot { get; set; }

    public PrivateKey(byte[] skSeed, byte[] skPrf, byte[]
        pkSeed, byte[] pkRoot)
    {
        SkSeed = skSeed;
        SkPrf = skPrf;
        PkSeed = pkSeed;
        PkRoot = pkRoot;
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
