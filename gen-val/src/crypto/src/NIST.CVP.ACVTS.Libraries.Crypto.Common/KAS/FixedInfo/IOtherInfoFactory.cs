using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo
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
        /// <param name="thisPartyOtherInfo">This party's Other info contribution</param>
        /// <param name="otherPartyOtherInfo">The other party's other info contribution</param>
        /// <returns></returns>
        IOtherInfo GetInstance(
            string otherInfoPattern,
            int otherInfoLength,
            KeyAgreementRole thisPartyKeyAgreementRole,
            PartyOtherInfo thisPartyOtherInfo,
            PartyOtherInfo otherPartyOtherInfo
        );
    }
}
