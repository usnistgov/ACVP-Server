using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TPMv1_2;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TPM
{
    public class ParameterBuilder
    {
        private readonly string _algorithm;
        private readonly string _mode;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "tpm";
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode
            };
        }
    }
}
