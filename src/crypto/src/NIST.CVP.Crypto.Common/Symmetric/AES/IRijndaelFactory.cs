namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IRijndaelFactory
    {
        IRijndael GetRijndael(ModeValues mode);
    }
}
