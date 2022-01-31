namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC
{
    public interface IKmacFactory
    {
        IKmac GetKmacInstance(int capacity, bool xof);
    }
}
