namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ISHA3Factory
    {
        ISHA3Wrapper GetSHA(HashFunction hashFunction);
    }
}
