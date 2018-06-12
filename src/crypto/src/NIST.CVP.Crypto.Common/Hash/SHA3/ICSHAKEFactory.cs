namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ICSHAKEFactory
    {
        ICSHAKEWrapper GetCSHAKE(HashFunction hashFunction);
    }
}
