namespace NIST.CVP.Crypto.SHA3
{
    public interface ISHA3Factory
    {
        SHA3Wrapper GetSHA(HashFunction hashFunction);
    }
}
