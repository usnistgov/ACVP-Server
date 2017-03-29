namespace NIST.CVP.Generation.DRBG
{
    public interface IDrbgFactory
    {
        IDrbg GetDrbgInstance(DrbgParameters drbgParameters);
    }
}