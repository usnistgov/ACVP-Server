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
using System.Collections.Concurrent;
using Autofac;
public interface IValidationResolver
{
    (IValidator Validator, ILifetimeScope Scope) Resolve(AlgoMode algoMode);
}
public class ValidationResolver : IValidationResolver, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<AlgoMode, IContainer> _containers = new();
    private bool _disposed;

    public ValidationResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
    }

    public (IValidator Validator, ILifetimeScope Scope) Resolve(AlgoMode algoMode)
    {
        var container = _containers.GetOrAdd(algoMode, mode =>
        {
            AutofacConfig.IoCConfiguration(_serviceProvider, mode);
            return AutofacConfig.GetContainer();
        });

        var scope = container.BeginLifetimeScope();
        var validator = scope.Resolve<IValidator>();

        return (validator, scope);
    }

    public void Dispose()
    {
        if (_disposed) return;

        foreach (var container in _containers.Values)
        {
            container.Dispose();
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}