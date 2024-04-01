using System.Collections;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;

public interface IMLDSA
{
    public (byte[] pk, byte[] sk) GenerateKey(BitArray seed);
    public byte[] Sign(byte[] sk, BitArray message, bool deterministic);
    public bool Verify(byte[] pk, byte[] signature, BitArray message);
}
