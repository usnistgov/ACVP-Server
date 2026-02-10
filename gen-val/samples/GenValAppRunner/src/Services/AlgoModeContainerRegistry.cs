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
using System.Threading;
using System.Collections.Concurrent;
using GenValAppRunner.DTO;
using Autofac;

public interface IAlgoModeContainerRegistry
{
    ILifetimeScope BeginScope(AlgoMode mode);
}

public class AlgoModeContainerRegistry: IAlgoModeContainerRegistry
{
    private readonly IReadOnlyDictionary<AlgoMode, IContainer> _containers;

    public AlgoModeContainerRegistry(IServiceProvider root)
    {

        _containers = AutofacConfig.GetSupportedAlgoModes()
        .ToDictionary(
          mode => mode,
          mode =>
        {
            AutofacConfig.IoCConfiguration(root, mode);
            return AutofacConfig.GetContainer();
        });

    }

    public ILifetimeScope BeginScope(AlgoMode mode)
        => _containers[mode].BeginLifetimeScope();
}
