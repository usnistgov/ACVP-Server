namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKEFactory
    {
        ICSHAKEWrapper GetCSHAKE(HashFunction hashFunction);
    }
}
