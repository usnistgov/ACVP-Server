using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    /// <summary>
    /// Interface for describing Key Confirmation
    /// 
    /// U = key agreement initiator
    /// V = key agreement responder
    /// </summary>
    public interface IKeyConfirmation
    {
        /// <summary>
        /// Computes a MAC for KAS
        /// </summary>
        /// <returns></returns>
        ComputeKeyMacResult ComputeKeyMac();

        ComputeKeyMacResult ConfirmOtherPartyMac(BitString otherPartyMac);
    }

}