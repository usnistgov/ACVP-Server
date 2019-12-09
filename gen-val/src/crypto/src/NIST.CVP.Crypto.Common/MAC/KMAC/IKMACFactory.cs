namespace NIST.CVP.Crypto.Common.MAC.KMAC
{
    public interface IKmacFactory
    {
        IKmac GetKmacInstance(int capacity, bool xof);
    }
}
