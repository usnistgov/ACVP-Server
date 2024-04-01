namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;

public interface IDilithiumFactory
{
    IMLDSA GetDilithium(DilithiumParameterSet parameterSet);
}
