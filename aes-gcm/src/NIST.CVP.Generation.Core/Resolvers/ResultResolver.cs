namespace NIST.CVP.Generation.Core.Resolvers
{
    public class ResultResolver: ResolverBase
    {
        public static readonly ResultResolver Instance = new ResultResolver();

        protected override string[] IgnoreProperties
        {
            get { return new[] {"testgroups", "answerprojection"}; }
        }
    }
}
