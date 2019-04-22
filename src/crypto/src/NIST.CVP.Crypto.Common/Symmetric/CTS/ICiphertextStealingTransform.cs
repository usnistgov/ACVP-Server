using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.CTS
{
    /// <summary>
    /// Used to apply a ciphertext stealing mode's final block(s) transform.
    /// </summary>
    public interface ICiphertextStealingTransform
    {
        BitString Transform(BitString ciphertext);
    }
}