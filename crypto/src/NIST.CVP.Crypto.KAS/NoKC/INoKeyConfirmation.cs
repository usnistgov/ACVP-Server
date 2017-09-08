using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public interface INoKeyConfirmation
    {
        ComputeKeyMacResult ComputeMac();
    }
}