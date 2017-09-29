namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class KdfNoKcBuilder
    {
        private ParameterSets _parameterSets;
        private KdfOptions _kdfOptions;
        
        public KdfNoKcBuilder()
        {
            _parameterSets = new ParameterSetBuilder(true).BuildParameterSets();
            _kdfOptions = new KdfOptionsBuilder().BuildKdfOptions();
        }

        public KdfNoKcBuilder WithParameterSets(ParameterSets value)
        {
            _parameterSets = value;
            return this;
        }

        public KdfNoKcBuilder WithKdfOptions(KdfOptions value)
        {
            _kdfOptions = value;
            return this;
        }

        public KdfNoKc BuildKdfNoKc()
        {
            return new KdfNoKc()
            {
                ParameterSet = _parameterSets,
                KdfOption = _kdfOptions
            };
        }
    }
}