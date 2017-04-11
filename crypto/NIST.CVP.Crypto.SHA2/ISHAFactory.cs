namespace NIST.CVP.Crypto.SHA2
{
    public interface ISHAFactory
    {
        ISHABase GetSHA(HashFunction hashFunction);
    }
}
