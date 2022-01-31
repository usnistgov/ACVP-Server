using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.EccComponent
{
    public class ParameterBuilder
    {
        private string _algorithm = "KAS-ECC";
        private string _mode = "CDH-Component";
        private string[] _function = ParameterValidator.ValidFunctions;
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

        public ParameterBuilder WithFunction(string[] value)
        {
            _function = value;
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
                Mode = _mode,
                Function = _function,
                Curve = _curves
            };
        }

    }
}
