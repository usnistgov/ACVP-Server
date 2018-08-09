using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;
using Orleans;
using Orleans.Providers;

namespace NIST.CVP.Orleans.Grains
{
    /// <summary>
    /// Base class for Oracle Grains.
    ///
    /// <see cref="DoWorkAsync"/> is implemented for performing the actual CPU bound work.
    ///     <see cref="Result"/> should be updated within this method.
    ///     <see cref="GrainState"/> should be updated to <see cref="GrainState.CompletedWork"/> within this method.
    ///
    /// The implementation class should utilize instance variables
    ///     to accommodate <see cref="DoWorkAsync"/>
    /// </summary>
    /// <example>
    /// Sample implementation:
    /// <code>
    ///
    ///     public async Task{T} MyGrainMethod(Parameters param)
    ///     {
    ///         _param = param;
    ///         await StateHandling();
    ///         return Result;
    ///     }
    /// 
    /// </code>
    /// </example>
    /// <typeparam name="TResult"></typeparam>
    [StorageProvider(ProviderName = Constants.StorageProviderName)]
    public abstract class OracleGrainBase<TResult> : Grain<GrainState>
    {
        protected TResult Result;

        /// <summary>
        /// Used to encapsulate the boilerplate state handling per grain.
        /// Should be invoked via actual grain implementation
        /// </summary>
        protected virtual async Task<TResult> StateHandling()
        {
            switch (State)
            {
                case GrainState.Faulted:
                    throw new NotSupportedException(
                        $"{this} is in state {State} and not available for further invocations."
                    );
                case GrainState.Initialized:
                    State = GrainState.Working;
                    await WriteStateAsync();

                    Task.Run(() =>
                    {
                        DoWorkAsync().FireAndForget();
                    }).FireAndForget();

                    return default(TResult);
                case GrainState.Working:
                    return default(TResult);
                case GrainState.CompletedWork:
                    State = GrainState.ShouldDispose;
                    await WriteStateAsync();
                    
                    return Result;
                default:
                    throw new ArgumentException($"Unexpected {nameof(State)}");
            }
        }

        /// <summary>
        /// The CPU bound work to do related to the grain.
        /// Implementation should update state to <see cref="GrainState.CompletedWork"/>
        /// when finished.
        /// </summary>
        /// <returns></returns>
        protected abstract Task DoWorkAsync();
    }
}
