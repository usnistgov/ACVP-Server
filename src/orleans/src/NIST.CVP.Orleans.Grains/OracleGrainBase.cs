using System;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;
using Orleans;
using Orleans.Providers;

namespace NIST.CVP.Orleans.Grains
{
    /// <summary>
    /// Base class for Pollable Oracle Grains.
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
    ///     public async Task{bool} MyGrainMethod(Parameters param)
    ///     {
    ///         _param = param;
    ///         return await BeginGrainWorkAsync();
    ///     }
    /// 
    /// </code>
    /// </example>
    /// <typeparam name="TResult">The type the grain returns</typeparam>
    [StorageProvider(ProviderName = Constants.StorageProviderName)]
    public abstract class OracleGrainBase<TResult> : Grain<GrainState>, IPollableOracleGrain<TResult>
    {
        protected TResult Result;
        protected readonly LimitedConcurrencyLevelTaskScheduler Scheduler;

        protected OracleGrainBase(LimitedConcurrencyLevelTaskScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        /// <summary>
        /// Kicks off <see cref="DoWorkAsync"/>, this method should be invoked
        /// via actual grain implementation after saving necessary parameters as members of the instance.
        /// This is done to keep a consistent task execution abstraction, one place to update if it's ever
        /// changed.
        /// </summary>
        /// <remarks>Calls <see cref="DoWorkAsync"/></remarks>
        /// <returns></returns>
        protected Task<bool> BeginGrainWorkAsync()
        {
            if (State == GrainState.Initialized)
            {
                State = GrainState.Working;

                Task.Factory.StartNew(() =>
                {
                    DoWorkAsync().FireAndForget();
                }, CancellationToken.None, TaskCreationOptions.None, scheduler: Scheduler).FireAndForget();
                
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// <summary>
        /// The CPU bound work to do related to the grain.
        /// Many grain methods will have differing parameters required,
        /// those parameters should be saved to the instance for utilization within <see cref="DoWorkAsync"/>.
        ///
        /// <see cref="Result"/> should be set with the outcome of this method,
        /// and State set to <see cref="GrainState.CompletedWork"/>
        /// </summary>
        /// <remarks>Invoked via <see cref="BeginGrainWorkAsync"/></remarks>
        /// <returns></returns>
        protected abstract Task DoWorkAsync();
        
        /// <summary>
        /// Returns the grain state
        /// </summary>
        /// <returns></returns>
        public Task<GrainState> CheckStatusAsync()
        {
            if (Result != null)
            {
                State = GrainState.CompletedWork;
            }

            return Task.FromResult(State);
        }

        /// <summary>
        /// Gets the result from the grain when available.  Should only be called once polling
        /// confirms calculation is complete.
        /// </summary>
        /// <returns></returns>
        public Task<TResult> GetResultAsync()
        {
            if (State != GrainState.CompletedWork)
            {
                throw new NotSupportedException(
                    $"Invalid State for returning result, must be in state {nameof(GrainState.CompletedWork)} to return result");
            }

            State = GrainState.ShouldDispose;
            return Task.FromResult(Result);
        }
    }
}
