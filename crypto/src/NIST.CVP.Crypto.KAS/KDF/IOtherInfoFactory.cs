using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.KDF
{
    /// <summary>
    /// Returns an instance of <see cref="IOtherInfo"/>
    /// </summary>
    public interface IOtherInfoFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="IOtherInfo"/> with the specified pattern
        /// </summary>
        /// <param name="otherInfoPattern">The pattern used to construct other information</param>
        /// <param name="otherInfoLength">The final length of other information</param>
        /// <param name="thisPartyKeyAgreementRole">This party's key aggreement role</param>
        /// <param name="thisPartySharedInformation">This party's public information</param>
        /// <param name="otherPartySharedInformation">The other party's public information</param>
        /// <returns></returns>
        IOtherInfo GetInstance(string otherInfoPattern, int otherInfoLength, KeyAgreementRole thisPartyKeyAgreementRole, FfcSharedInformation thisPartySharedInformation, FfcSharedInformation otherPartySharedInformation);
    }
}