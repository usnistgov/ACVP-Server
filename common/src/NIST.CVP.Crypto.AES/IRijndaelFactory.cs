namespace NIST.CVP.Crypto.AES
{
    public interface IRijndaelFactory
    {
        Rijndael GetRijndael(ModeValues mode);
    }
}
