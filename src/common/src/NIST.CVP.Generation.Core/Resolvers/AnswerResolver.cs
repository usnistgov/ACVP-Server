namespace NIST.CVP.Generation.Core.Resolvers
{
    public class AnswerResolver: ResolverBase
    {
        public static readonly AnswerResolver Instance = new AnswerResolver();

        protected override string[] IgnoreProperties
        {
            get { return new[] {"testgroups", "testresults"}; }
        }
    }
}
