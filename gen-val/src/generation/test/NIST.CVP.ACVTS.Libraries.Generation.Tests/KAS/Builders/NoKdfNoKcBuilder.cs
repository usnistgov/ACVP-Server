using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.Builders
{
    public class NoKdfNoKcBuilder
    {
        protected ParameterSets ParameterSets;

        public NoKdfNoKcBuilder()
        {
            ParameterSets = new ParameterSetBuilder(false).BuildParameterSets();
        }

        public NoKdfNoKcBuilder WithParameterSets(ParameterSets value)
        {
            ParameterSets = value;
            return this;
        }

        public NoKdfNoKc BuildNoKdfNoKc()
        {
            return new NoKdfNoKc()
            {
                ParameterSet = ParameterSets
            };
        }
    }
}
