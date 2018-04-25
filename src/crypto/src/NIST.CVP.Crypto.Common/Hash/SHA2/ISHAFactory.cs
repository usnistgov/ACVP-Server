namespace NIST.CVP.Crypto.Common.Hash.SHA2
{
    public interface ISHAFactory
    {
        ISHABase GetSHA(HashFunction hashFunction);
    }
}
