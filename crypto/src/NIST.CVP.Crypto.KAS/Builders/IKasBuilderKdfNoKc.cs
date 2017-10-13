using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderKdfNoKc
    {
        IKas Build();
        IKasBuilderKdfNoKc WithKeyLength(int value);
        IKasBuilderKdfNoKc WithMacParameters(MacParameters value);
        IKasBuilderKdfNoKc WithOtherInfoPattern(string value);
    }
}