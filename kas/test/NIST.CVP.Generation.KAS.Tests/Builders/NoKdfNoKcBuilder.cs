namespace NIST.CVP.Generation.KAS.Tests.Builders
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