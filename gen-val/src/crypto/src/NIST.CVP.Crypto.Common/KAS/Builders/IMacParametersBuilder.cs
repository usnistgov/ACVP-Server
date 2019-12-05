using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Builders
{
    public interface IMacParametersBuilder
    {
        MacParameters Build();
        IMacParametersBuilder WithKeyAgreementMacType(KeyAgreementMacType value);
        IMacParametersBuilder WithMacLength(int value);
        IMacParametersBuilder WithNonce(BitString value);
    }
}