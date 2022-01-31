using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders
{
    public interface IMacParametersBuilder
    {
        MacParameters Build();
        IMacParametersBuilder WithKeyAgreementMacType(KeyAgreementMacType value);
        IMacParametersBuilder WithKeyLength(int value);
        IMacParametersBuilder WithMacLength(int value);
        IMacParametersBuilder WithNonce(BitString value);
    }
}
