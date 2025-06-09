using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using System.Threading.Tasks;
using Autofac;

public interface IGeneratorResolver
{
    (IGenerator Generator, ILifetimeScope Scope) Resolve(AlgoMode algoMode);
}


public class GeneratorResolver : IGeneratorResolver
{
    private readonly IServiceProvider _serviceProvider;

    public GeneratorResolver()
    {
        _serviceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
    }

    public (IGenerator Generator, ILifetimeScope Scope) Resolve(AlgoMode algoMode)
    {
        // Rebuild Autofac container for specific algoMode
        AutofacConfig.IoCConfiguration(_serviceProvider, algoMode);
        var container = AutofacConfig.GetContainer();
        var scope = container.BeginLifetimeScope();
             
        // Resolve the generator inside the scope
        var generator = scope.Resolve<IGenerator>();

        return (generator, scope);
    }
}
