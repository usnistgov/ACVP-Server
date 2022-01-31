using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.Builders
{
    public class ParameterSetBuilder
    {
        private Fb _fb;
        private Fc _fc;

        public ParameterSetBuilder(bool includeMacInParameterSet)
        {
            _fb = new ParameterSetBaseBuilder(includeMacInParameterSet).BuildParameterSetBaseFb();
            _fc = new ParameterSetBaseBuilder(includeMacInParameterSet).BuildParameterSetBaseFc();
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

        public ParameterSets BuildParameterSets()
        {
            return new ParameterSets()
            {
                Fb = _fb,
                Fc = _fc
            };
        }
    }
}
