namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;

public interface IMLKEM
{
    public (byte[] ek, byte[] dk) GenerateKey(byte[] z, byte[] d);
    public (byte[] K, byte[] c) Encapsulate(byte[] ek, byte[] m);
    public (byte[] sharedKey, bool implicitRejection) Decapsulate(byte[] dk, byte[] c);
}
