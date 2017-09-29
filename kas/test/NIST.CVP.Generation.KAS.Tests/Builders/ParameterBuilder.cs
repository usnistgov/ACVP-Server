using SQLitePCL;

namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _functions;
        private Schemes _schemes;

        public ParameterBuilder()
        {
            _algorithm = "KAS-FFC";
            _functions = ParameterValidator.ValidFunctions;
            _schemes = new SchemesBuilder().BuildSchemes();
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
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
                Function = _functions,
                Scheme = _schemes
            };
        }
    }
}