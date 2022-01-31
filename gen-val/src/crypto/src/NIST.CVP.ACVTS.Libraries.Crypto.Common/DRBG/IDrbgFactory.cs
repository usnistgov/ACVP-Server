using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG
{
    /// <summary>
    /// Provides a means to retrieve a DRBG implementation
    /// </summary>
    public interface IDrbgFactory
    {
        IDrbg GetDrbgInstance(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider);
    }
}
