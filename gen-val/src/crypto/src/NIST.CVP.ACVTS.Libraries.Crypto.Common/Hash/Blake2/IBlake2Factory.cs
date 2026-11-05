namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2
{
    public interface IBlake2Factory
    {
        IBlake2 GetBlake2Instance(Blake2HashFunction hashFunction);
    }
}
