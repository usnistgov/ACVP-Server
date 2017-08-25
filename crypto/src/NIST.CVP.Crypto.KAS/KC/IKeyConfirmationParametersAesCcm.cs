using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public interface IKeyConfirmationParametersAesCcm
    {
        /// <summary>
        /// The <see cref="IAES_CCM"/> nonce
        /// </summary>
        BitString Nonce { get; }
    }
}