namespace NIST.CVP.ACVTS.Libraries.Crypto.Ascon;

public class AsconDecryptResult
{
    public byte[] Result { get; set; }
    public bool HasResult => Result != null;

    public AsconDecryptResult(byte[] plaintext)
    {
        Result = plaintext;
    }

    public AsconDecryptResult() { }
}
