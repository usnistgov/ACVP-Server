using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    /// <summary>
    /// Tests used to ensure runtime loader is able to resolve each permutation of <see cref="AlgoMode"/>, 
    /// as well as each permutation of <see cref="AlgorithmDllDependencies"/> from appSettings.
    /// 
    /// Note in order for these tests to pass, all algorithms must be built and available for runtime loading
    /// from NIST.CVP.Generation.GenValApp/dlls/*
    /// </summary>
    [TestFixture, FastIntegrationTest]
    public class RuntimeLoaderTests
    {
        private AlgorithmConfig _config;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _config = GenValApp.Program.ServiceProvider.GetService<IOptions<AlgorithmConfig>>().Value;
        }
        
        [Test]
        public void ShouldResolveGeneratorAndValidatorForEachAppSettingsEnumeration()
        {
            try
            {
                foreach (var algoInfo in _config.Algorithms)
                {
                    ResolveGenVal(algoInfo);
                }
            }
            catch (Exception ex)
            {
                string message = $"{ex.Message}\n{ex.StackTrace}";
                ThisLogger.Error(message);
                Assert.Fail(message);
            }

            Assert.Pass();
        }

        [Test]
        public void ShouldResolveGeneratorAndValidatorForEachAlgoModeEnumeration()
        {
            try
            {
                foreach (AlgoMode algoMode in Enum.GetValues(typeof(AlgoMode)))
                {
                    var enumDesc = EnumHelpers.GetEnumDescriptionFromEnum(algoMode);
                    if (!_config.Algorithms.TryFirst(t =>
                            // Check if {algo}-{mode} equals enumDesc
                            // or in cases of empty mode, just compare against the {algo}
                            $"{t.Algorithm}-{t.Mode}".Equals(enumDesc, StringComparison.OrdinalIgnoreCase) 
                            || (
                                t.Algorithm.Equals(enumDesc, StringComparison.OrdinalIgnoreCase) 
                                && string.IsNullOrEmpty(t.Mode)
                             ),
                        out var algoInfo)
                    )
                    {
                        Assert.Fail($"Unable to find {nameof(enumDesc)} {enumDesc} in {nameof(_config)}");
                    }

                    ResolveGenVal(algoInfo);
                }
            }
            catch (Exception ex)
            {
                string message = $"{ex.Message}\n{ex.StackTrace}";
                ThisLogger.Error(message);
                Assert.Fail(message);
            }

            Assert.Pass();
        }

        private void ResolveGenVal(AlgorithmDllDependencies algoInfo)
        {
            AutofacConfig.IoCConfiguration(Program.ServiceProvider, algoInfo.Algorithm, algoInfo.Mode, Program.RootDirectory);
            using (var scope = AutofacConfig.GetContainer().BeginLifetimeScope())
            {
                var gen = scope.Resolve<IGenerator>();
                var val = scope.Resolve<IValidator>();

                Assert.IsNotNull(gen, nameof(gen));
                Assert.IsNotNull(val, nameof(val));
            }
        }

        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}