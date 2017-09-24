namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class ParameterSetBuilder
    {
        private Fb _fb;
        private Fc _fc;

        public ParameterSetBuilder(bool includeMacInParameterSet)
        {
            _fb = new ParameterSetBaseBuilder(includeMacInParameterSet).BuildFb();
            _fc = new ParameterSetBaseBuilder(includeMacInParameterSet).BuildFc();
        }

        public ParameterSetBuilder WithFb(Fb value)
        {
            _fb = value;
            return this;
        }

        public ParameterSetBuilder WithFc(Fc value)
        {
            _fc = value;
            return this;
        }

        public ParameterSets Build()
        {
            return new ParameterSets()
            {
                Fb = _fb,
                Fc = _fc
            };
        }
    }
}