namespace NIST.CVP.Crypto.Common.KAS.NoKC
{
    public interface INoKeyConfirmation
    {
        ComputeKeyMacResult ComputeMac();
    }
}