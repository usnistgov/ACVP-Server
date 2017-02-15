namespace NIST.CVP.Generation.SHA2
{
    public interface ISHAFactory
    {
        ISHABase GetSHA(HashFunction hashFunction);
    }
}
