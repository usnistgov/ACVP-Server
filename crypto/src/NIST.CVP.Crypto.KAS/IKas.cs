using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    /// <summary>
    /// Interface for Key Agreement Schemes
    /// </summary>
    public interface IKas
    {
        IScheme Scheme { get; }

        void SetDomainParameters(FfcDomainParameters domainParameters);
        /// <summary>
        /// Gets the shared information needed by the other party to complete key agreement
        /// </summary>
        /// <returns></returns>
        FfcSharedInformation ReturnPublicInfoThisParty();
        /// <summary>
        /// The result of the key agreement attempt
        /// </summary>
        /// <param name="otherPartySharedInformation">The other party's shared information</param>
        /// <returns></returns>
        KasResult ComputeResult(FfcSharedInformation otherPartySharedInformation);
    }
    
}