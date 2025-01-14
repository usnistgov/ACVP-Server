using System.Collections;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;

public interface IMLDSA
{
    public (byte[] pk, byte[] sk) GenerateKey(BitArray seed);
    public byte[] Sign(byte[] sk, byte[] message, byte[] rnd);
    public bool Verify(byte[] pk, byte[] signature, byte[] message);
}
