namespace NIST.CVP.Generation.Core.Resolvers
{
    public class PromptResolver:ResolverBase
    {
        public static readonly PromptResolver Instance = new PromptResolver();

        protected override string[] IgnoreProperties
        {
            get { return new[] { "answerprojection", "testresults", "issample" }; }
        }
    }
}
