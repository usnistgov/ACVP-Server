using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using Orleans;

namespace NIST.CVP.Orleans.Grains
{
    /// <summary>
    /// Base class for observable grains.
    ///
    /// <see cref="DoWorkAsync"/> is implemented for performing the actual CPU bound work.
    /// <see cref="Notify" /> should be invoked upon completion of CPU bound work.
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
    /// 
    /// <typeparam name="TResult">The type the grain notifies subscribers of.</typeparam>
    public abstract class ObservableOracleGrainBase<TResult> : Grain, IGrainMarker, IGrainObservable<TResult>
    {
        /// <summary>
        /// CPU bound work <see cref="TaskScheduler"/>
        /// </summary>
        protected readonly LimitedConcurrencyLevelTaskScheduler NonOrleansScheduler;
        /// <summary>
        /// The <see cref="TaskScheduler"/> utilized via Orleans
        /// </summary>
        protected TaskScheduler OrleansScheduler;
        /// <summary>
        /// The collection to keep track of subscribers
        /// </summary>
        private GrainObserverManager<IGrainObserver<TResult>> _subsManager;
        /// <summary>
        /// Monitors if work has begun via a call to <see cref="BeginGrainWorkAsync"/>.
        /// If the original working node suicides, because we are using stateless grains, the expectation is
        /// <see cref="HeartbeatSubscribe"/>s will continue without work actually being set up on the grain.
        ///
        /// If this is encountered, we can throw an exception within Subscribe to the caller, indicating the work should
        /// be resubmitted.
        /// </summary>
        private bool _hasWorkBegun = false;

        protected ObservableOracleGrainBase(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler)
        {
            NonOrleansScheduler = nonOrleansScheduler;
        }

        public override async Task OnActivateAsync()
        {
            OrleansScheduler = TaskScheduler.Current;
            _subsManager = new GrainObserverManager<IGrainObserver<TResult>>();
            
            await base.OnActivateAsync();
        }

        public Task InitialSubscribe(IGrainObserver<TResult> observer)
        {
            _subsManager.Subscribe(observer);
            return Task.CompletedTask;
        }

        public Task HeartbeatSubscribe(IGrainObserver<TResult> observer)
        {
            if (!_hasWorkBegun)
            {
                _subsManager.Subscribe(observer);
                _subsManager.Notify(o => o.Throw(new OriginalClusterNodeSuicideException()));
                return Task.CompletedTask;
            }
            
            _subsManager.Subscribe(observer);
            return Task.CompletedTask;
        }

        public Task Unsubscribe(IGrainObserver<TResult> observer)
        {
            _subsManager.Unsubscribe(observer);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Fires off CPU bound work onto a separate task scheduler <see cref="NonOrleansScheduler"/>
        /// as to not block the Orleans task scheduler <see cref="OrleansScheduler"/>
        /// </summary>
        /// <returns></returns>
        protected Task<bool> BeginGrainWorkAsync()
        {
            _hasWorkBegun = true;
            
            Task.Factory.StartNew(() =>
            {
                DoWorkAsyncWrapper().FireAndForget();
            }, CancellationToken.None, TaskCreationOptions.None, NonOrleansScheduler).FireAndForget();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Wraps the call to <see cref="DoWorkAsync"/> in a try/catch to handle exceptions.
        /// Exceptions weren't being bubbled up if they would occur without this method -
        /// unless handling in each individual grain.
        /// </summary>
        /// <returns></returns>
        private async Task DoWorkAsyncWrapper()
        {
            try
            {
                await DoWorkAsync();
            }
            catch (Exception ex)
            {
                await Throw(ex);
            }
        }

        /// <summary>
        /// The CPU bound work to do related to the grain.
        /// Many grain methods will have differing parameters required,
        /// those parameters should be saved to the instance for utilization within <see cref="DoWorkAsync"/>.
        ///
        /// Implementor should <see cref="Notify"/> on completion.
        /// </summary>
        /// <remarks>Invoked via <see cref="BeginGrainWorkAsync"/></remarks>
        /// <returns></returns>
        protected abstract Task DoWorkAsync();

        /// <summary>
        /// Sends notification to subscribed observers about computed result.
        /// This notification is scheduled on the <see cref="OrleansScheduler"/>
        /// as per requirements.
        ///
        /// Caller should invoke within <see cref="DoWorkAsync"/>
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected async Task Notify(TResult result)
        {
            await Task.Factory.StartNew(() =>
            {
                while (!_subsManager.Any())
                {
                    Task.Delay(5000);
                }
                
                _subsManager.Notify(observer => observer.ReceiveMessageFromCluster(result));
            }, CancellationToken.None, TaskCreationOptions.None, OrleansScheduler);
        }

        /// <summary>
        /// Sends notification to subscribed observers about exception.
        /// This notification is scheduled on the <see cref="OrleansScheduler"/>
        /// as per requirements.
        ///
        /// Caller should invoke within <see cref="DoWorkAsync"/>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected async Task Throw(Exception exception)
        {
            await Task.Factory.StartNew(() =>
            {
                while (!_subsManager.Any())
                {
                    Task.Delay(5000);
                }
            
                _subsManager.Notify(observer => observer.Throw(exception));
            }, CancellationToken.None, TaskCreationOptions.None, OrleansScheduler);
        }
    }
}