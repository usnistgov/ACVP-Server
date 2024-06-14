using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;

public class PublicKey
{
    public byte[] PkSeed { get; set; }
    public byte[] PkRoot { get; set; }

    public PublicKey(byte[] pkSeed, byte[] pkRoot)
    {
        PkSeed = pkSeed;
        PkRoot = pkRoot;
    }
    
    public byte[] GetBytes()
    {
        var publicKeyBytes = new byte[PkSeed.Length + PkRoot.Length];
        Array.Copy(PkSeed, 0, publicKeyBytes, 0, PkSeed.Length);
        Array.Copy(PkRoot, 0, publicKeyBytes, PkSeed.Length, PkRoot.Length);
        
        return publicKeyBytes;
    }
}
