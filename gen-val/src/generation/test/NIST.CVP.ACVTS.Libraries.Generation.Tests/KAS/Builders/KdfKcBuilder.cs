using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.Builders
{
    public class KdfKcBuilder
    {
        private ParameterSets _parameterSets;
        private KdfOptions _kdfOptions;
        private KcOptions _kcOptions;

        public KdfKcBuilder()
        {
            _parameterSets = new ParameterSetBuilder(true).BuildParameterSets();
            _kdfOptions = new KdfOptionsBuilder().BuildKdfOptions();
            _kcOptions = new KcOptionsBuilder().BuildKcOptions();
        }

        public KdfKcBuilder WithParameterSets(ParameterSets value)
        {
            _parameterSets = value;
            return this;
        }

        public KdfKcBuilder WithKdfOptions(KdfOptions value)
        {
            _kdfOptions = value;
            return this;
        }

        public KdfKcBuilder WithKcOptions(KcOptions value)
        {
            _kcOptions = value;
            return this;
        }

        public KdfKc Build()
        {
            return new KdfKc()
            {
                ParameterSet = _parameterSets,
                KdfOption = _kdfOptions,
                KcOption = _kcOptions
            };
        }
    }
}
