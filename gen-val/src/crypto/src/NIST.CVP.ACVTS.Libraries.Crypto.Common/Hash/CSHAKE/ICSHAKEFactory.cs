namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE
{
    public interface IcSHAKEFactory
    {
        IcSHAKEWrapper GetcSHAKE(HashFunction hashFunction);
    }
}
