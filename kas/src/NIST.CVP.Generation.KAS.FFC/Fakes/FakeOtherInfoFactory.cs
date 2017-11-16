using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC.Fakes
{
    public class FakeOtherInfoFactory<TSharedInformation, TDomainParameters, TKeyPair> : IOtherInfoFactory<TSharedInformation, TDomainParameters, TKeyPair>
        where TSharedInformation : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        private readonly BitString _otherInfo;

        public FakeOtherInfoFactory(BitString otherInfo)
        {
            _otherInfo = otherInfo;
        }

        public IOtherInfo GetInstance(
            string otherInfoPattern, 
            int otherInfoLength, 
            KeyAgreementRole thisPartyKeyAgreementRole,
            TSharedInformation thisPartySharedInformation,
            TSharedInformation otherPartySharedInformation)
        {
            return new FakeOtherInfo(_otherInfo);
        }
    }

    internal class FakeOtherInfo : IOtherInfo
    {
        private readonly BitString _otherInfo;

        public FakeOtherInfo(BitString otherInfo)
        {
            _otherInfo = otherInfo;
        }

        public BitString GetOtherInfo()
        {
            return _otherInfo;
        }
    }
}