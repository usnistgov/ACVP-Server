namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _functions;
        private Schemes _schemes;

        public ParameterBuilder()
        {
            _algorithm = "KAS";
            _mode = "FFC";
            _functions = new string[]
            {
                "dpGen",
                "dpVal",
                "keyPairGen",
                "fullVal",
                "keyRegen"
            };
            _schemes = new SchemesBuilder().BuildSchemes();
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithFunctions(string[] value)
        {
            _functions = value;
            return this;
        }

        public ParameterBuilder WithSchemes(Schemes value)
        {
            _schemes = value;
            return this;
        }

        public Parameters BuildParameters()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Function = _functions,
                Scheme = _schemes
            };
        }
    }
}