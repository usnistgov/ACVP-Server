using System.Collections.Generic;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Tests.Core;

namespace NIST.CVP.Generation.Core.Tests
{
    /// <summary>
    /// Used as a base gen/val integration test class for algorithms implementing single app runner
    /// </summary>
    public abstract class GenValTestsSingleRunnerBase : GenValTestsBase
    {
        /// <summary>
        /// The algorithm to pass into the runner.  Default value is <see cref="GenValTestsBase.Algorithm"/>
        /// </summary>
        public virtual string RunnerAlgorithm => Algorithm;

        /// <summary>
        /// The mode to pass into the runner.  Default value is <see cref="GenValTestsBase.Mode"/>
        /// </summary>
        public virtual string RunnerMode => Mode;

        public string DllDropLocation { get; private set; }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            DllDropLocation =
                Utilities.GetConsistentTestingStartPath(GetType(),
                    @"..\..\..\common\src\NIST.CVP.Generation.GenValApp\");
        }

        protected override string[] GetParameters(string[] parameters, GenValMode mode)
        {
            List<string> listParams = new List<string>
            {
                "-a",
                RunnerAlgorithm,
                "-m",
                RunnerMode,
                "-d",
                DllDropLocation
            };

            if (parameters.Length == 0)
            {
                return listParams.ToArray();
            }

            if (mode == GenValMode.Generate)
            {
                listParams.Add("-g");
                listParams.Add(parameters[0]);
            }
            else
            {
                listParams.Add("-r");
                listParams.Add(parameters[0]);
                listParams.Add("-p");
                listParams.Add(parameters[1]);
                listParams.Add("-n");
                listParams.Add(parameters[2]);
            }

            return listParams.ToArray();
        }
    }
}
