namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class ParameterSetBaseBuilder
    {
        private string[] _hashAlg;
        private MacOptions _macOptions;

        public ParameterSetBaseBuilder(bool includeMacInParameterSet)
        {
            _hashAlg = new string[] { "SHA2-512" };

            if (includeMacInParameterSet)
            {
                _macOptions = new MacOptionsBuilder().BuildMacOptions();
            }
        }

        public ParameterSetBaseBuilder WithHashAlg(string[] value)
        {
            _hashAlg = value;
            return this;
        }

        public ParameterSetBaseBuilder WithMacOptions(MacOptions value)
        {
            _macOptions = value;
            return this;
        }

        public Fb BuildParameterSetBaseFb()
        {
            return new Fb()
            {
                HashAlg = _hashAlg,
                MacOption = _macOptions
            };
        }

        public Fc BuildParameterSetBaseFc()
        {
            return new Fc()
            {
                HashAlg = _hashAlg,
                MacOption = _macOptions
            };
        }
    }
}