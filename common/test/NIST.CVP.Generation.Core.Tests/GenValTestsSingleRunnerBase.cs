using System.Collections.Generic;
using Autofac;
using NIST.CVP.Generation.Core.Tests.Enums;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    /// <summary>
    /// Used as a base gen/val integration test class for algorithms implementing single app runner
    /// </summary>
    public abstract class GenValTestsSingleRunnerBase<TParameters> : GenValTestsBase
        where TParameters : IParameters
    {
        /// <summary>
        /// The algorithm to pass into the runner.  Default value is <see cref="GenValTestsBase.Algorithm"/>
        /// </summary>
        public virtual string RunnerAlgorithm => Algorithm;

        /// <summary>
        /// The mode to pass into the runner.  Default value is <see cref="GenValTestsBase.Mode"/>
        /// </summary>
        public virtual string RunnerMode => Mode;

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<TParameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            GenValApp.Helpers.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override string[] GetParameters(string[] parameters, GenValMode mode)
        {
            List<string> listParams = new List<string>
            {
                "-a",
                RunnerAlgorithm,
                "-m",
                RunnerMode
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
