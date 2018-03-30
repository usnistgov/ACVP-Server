using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Generation.KAS.EccComponent.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm = "KAS-ECC";
        private string _mode = "Component";
        private string[] _curves = EnumHelpers.GetEnumDescriptions<Curve>().ToArray();

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

        public ParameterBuilder WithCurves(string[] value)
        {
            _curves = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                Curves = _curves,
                Mode = _mode
            };
        }

    }
}