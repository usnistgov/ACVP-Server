namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public enum DirectionValues
    {
        Encrypt,
        Decrypt
    }

    public enum ModeValues
    {
        ECB,
        CBC,
        OFB,
        CFB1,
        CFB8,
        CFB128,
        Counter,
        CBCMac,
        CMAC
    }
}
