namespace NIST.CVP.Crypto.DRBG
{
    public interface IDrbgFactory
    {
        IDrbg GetDrbgInstance(DrbgParameters drbgParameters);
    }
}