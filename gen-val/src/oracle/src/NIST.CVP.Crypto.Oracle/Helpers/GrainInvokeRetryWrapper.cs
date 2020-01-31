using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Crypto.Oracle.Exceptions;
using NLog;
using Orleans.Runtime.Messaging;

namespace NIST.CVP.Crypto.Oracle.Helpers
{
    /// <summary>
    /// Contains several generic methods that can be used to wrap grain calls in a while loop until x attempts attempted 
    /// due to GatewayTooBusyExceptions/TimeoutExceptions from the Orleans cluster.
    ///
    /// This GatewayTooBusyExceptions are thrown by the cluster when LoadShedding is enabled and the cluster's CPU exceeds
    /// the threshold amount.
    /// </summary>
    public static class GrainInvokeRetryWrapper
    {
        private static readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Wrap a grain call that returns a <see cref="Task"/> and takes a single parameter.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param">The parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain calls that failed due to LoadShedding or Timeout.</param>
        /// <returns>Task</returns>
        /// <exception cref="OrleansGrainFailedAfterRetries">Thrown when too many failed attempts of invoking grains occurs.</exception>
        public static async Task WrapGrainCall<T>(Func<T, Task> func, T param, int timesToRetry)
        {
            while (timesToRetry > 0)
            {
                timesToRetry--;

                try
                {
                    await func(param);
                    return;
                }
                catch (TimeoutException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(TimeoutException)} stack: {ex.StackTrace}");
                }
                catch (GatewayTooBusyException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(GatewayTooBusyException)} stack: {ex.StackTrace}");
                }
                catch (ConnectionFailedException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(ConnectionFailedException)} stack: {ex.StackTrace}.");
                }
                catch (SiloUnavailableException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(SiloUnavailableException)} stack: {ex.StackTrace}.");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
            }

            ThisLogger.Error("Too many failed attempts at invoking a grain.");
            throw new OrleansGrainFailedAfterRetries();
        }

        /// <summary>
        /// Wrap a grain call with no parameters.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain calls that failed due to LoadShedding or Timeout.</param>
        /// <returns>Task wrapped TResult</returns>
        /// <exception cref="OrleansGrainFailedAfterRetries">Thrown when too many failed attempts of invoking grains occurs.</exception>
        public static async Task WrapGrainCall<TResult>(Func<Task<TResult>> func, int timesToRetry)
        {
            while (timesToRetry > 0)
            {
                timesToRetry--;
                
                try
                {
                    await func();
                    return;
                }
                catch (TimeoutException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(TimeoutException)} stack: {ex.StackTrace}");
                }
                catch (GatewayTooBusyException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(GatewayTooBusyException)} stack: {ex.StackTrace}");
                }
                catch (ConnectionFailedException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(ConnectionFailedException)} stack: {ex.StackTrace}.");
                }
                catch (SiloUnavailableException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(SiloUnavailableException)} stack: {ex.StackTrace}.");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
            }
            
            ThisLogger.Error("Too many failed attempts at invoking a grain.");
            throw new OrleansGrainFailedAfterRetries();
        }

        /// <summary>
        /// Wrap a grain call that returns a <see cref="TResult"/> and takes a single parameter.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param">The parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain calls that failed due to LoadShedding or Timeout.</param>
        /// <returns>Task wrapped TResult</returns>
        /// <exception cref="OrleansGrainFailedAfterRetries">Thrown when too many failed attempts of invoking grains occurs.</exception>
        public static async Task<TResult> WrapGrainCall<T, TResult>(Func<T, Task<TResult>> func, T param, int timesToRetry)
        {
            while (timesToRetry > 0)
            {
                timesToRetry--;
                
                try
                {
                    return await func(param);
                }
                catch (TimeoutException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(TimeoutException)} stack: {ex.StackTrace}");
                }
                catch (GatewayTooBusyException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(GatewayTooBusyException)} stack: {ex.StackTrace}");
                }
                catch (ConnectionFailedException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(ConnectionFailedException)} stack: {ex.StackTrace}.");
                }
                catch (SiloUnavailableException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(SiloUnavailableException)} stack: {ex.StackTrace}.");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
            }
            
            ThisLogger.Error("Too many failed attempts at invoking a grain.");
            throw new OrleansGrainFailedAfterRetries();
        }

        /// <summary>
        /// Wrap a grain call that takes two parameters.
        /// </summary>
        /// <typeparam name="T1">The first parameter type.</typeparam>
        /// <typeparam name="T2">The second parameter type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param1">The first parameter to use with the function invoke.</param>
        /// <param name="param2">The second parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain calls that failed due to LoadShedding or Timeout.</param>
        /// <returns>Task wrapped TResult</returns>
        /// <exception cref="OrleansGrainFailedAfterRetries">Thrown when too many failed attempts of invoking grains occurs.</exception>
        public static async Task WrapGrainCall<T1, T2, TResult>(Func<T1, T2, Task<TResult>> func, T1 param1, T2 param2, int timesToRetry)
        {
            while (timesToRetry > 0)
            {
                timesToRetry--;
                
                try
                {
                    await func(param1, param2);
                    return;
                }
                catch (TimeoutException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(TimeoutException)} stack: {ex.StackTrace}");
                }
                catch (GatewayTooBusyException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(GatewayTooBusyException)} stack: {ex.StackTrace}");
                }
                catch (ConnectionFailedException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(ConnectionFailedException)} stack: {ex.StackTrace}.");
                }
                catch (SiloUnavailableException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(SiloUnavailableException)} stack: {ex.StackTrace}.");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
            }
            
            ThisLogger.Error("Too many failed attempts at invoking a grain.");
            throw new OrleansGrainFailedAfterRetries();
        }

        /// <summary>
        /// Wrap a grain call that takes three parameters.
        /// </summary>
        /// <typeparam name="T1">The first parameter type.</typeparam>
        /// <typeparam name="T2">The second parameter type.</typeparam>
        /// <typeparam name="T3">The third parameter type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param1">The first parameter to use with the function invoke.</param>
        /// <param name="param2">The second parameter to use with the function invoke.</param>
        /// <param name="param3">The third parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain calls that failed due to LoadShedding or Timeout.</param>
        /// <returns>Task wrapped TResult</returns>
        /// <exception cref="OrleansGrainFailedAfterRetries">Thrown when too many failed attempts of invoking grains occurs.</exception>
        public static async Task WrapGrainCall<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> func, T1 param1, T2 param2, T3 param3, int timesToRetry)
        {
            while (timesToRetry > 0)
            {
                timesToRetry--;
                
                try
                {
                    await func(param1, param2, param3);
                    return;
                }
                catch (TimeoutException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(TimeoutException)} stack: {ex.StackTrace}");
                }
                catch (GatewayTooBusyException ex)
                {
                    ThisLogger.Warn(ex, $"{nameof(GatewayTooBusyException)} stack: {ex.StackTrace}");
                }
                catch (ConnectionFailedException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(ConnectionFailedException)} stack: {ex.StackTrace}.");
                }
                catch (SiloUnavailableException ex)
                {
                    ThisLogger.Warn(ex,
                        $"{nameof(SiloUnavailableException)} stack: {ex.StackTrace}.");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
            }
            
            ThisLogger.Error("Too many failed attempts at invoking a grain.");
            throw new OrleansGrainFailedAfterRetries();
        }
    }
}
