using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Crypto.Oracle.Exceptions;

namespace NIST.CVP.Crypto.Oracle.Helpers
{
    /// <summary>
    /// Contains several generic methods that can be used to wrap grain calls in a recursive retry
    /// due to GatewayTooBusyExceptions from the Orleans cluster.
    ///
    /// This exception is thrown by the cluster when LoadShedding is enabled and the cluster's CPU exceeds
    /// the threshold amount.
    /// </summary>
    public static class GrainInvokeRetryWrapper
    {
        /// <summary>
        /// Wrap a grain call that returns a <see cref="Task"/> and takes a single parameter.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param">The parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain call's that failed due to LoadShedding</param>
        /// <returns>Task</returns>
        public static async Task WrapGrainCall<T>(Func<T, Task> func, T param, int timesToRetry)
        {
            if (timesToRetry < 0)
            {
                throw new OrleansGrainFailedAfterRetries();
            }

            timesToRetry--;

            try
            {
                await func(param);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                await WrapGrainCall(func, param, timesToRetry);
            }
        }

        /// <summary>
        /// Wrap a grain call that returns a <see cref="TResult"/> and no parameters.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain call's that failed due to LoadShedding</param>
        /// <returns>Task wrapped TResult</returns>
        public static async Task<TResult> WrapGrainCall<TResult>(Func<Task<TResult>> func, int timesToRetry)
        {
            if (timesToRetry < 0)
            {
                throw new OrleansGrainFailedAfterRetries();
            }

            timesToRetry--;

            try
            {
                return await func();
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                return await WrapGrainCall(func, timesToRetry);
            }
        }

        /// <summary>
        /// Wrap a grain call that returns a <see cref="TResult"/> and takes a single parameter.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param">The parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain call's that failed due to LoadShedding</param>
        /// <returns>Task wrapped TResult</returns>
        public static async Task<TResult> WrapGrainCall<T, TResult>(Func<T, Task<TResult>> func, T param, int timesToRetry)
        {
            if (timesToRetry < 0)
            {
                throw new OrleansGrainFailedAfterRetries();
            }

            timesToRetry--;

            try
            {
                return await func(param);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                return await WrapGrainCall(func, param, timesToRetry);
            }
        }

        /// <summary>
        /// Wrap a grain call that returns a <see cref="TResult"/> and takes two parameters.
        /// </summary>
        /// <typeparam name="T1">The first parameter type.</typeparam>
        /// <typeparam name="T2">The second parameter type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param1">The first parameter to use with the function invoke.</param>
        /// <param name="param2">The second parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain call's that failed due to LoadShedding</param>
        /// <returns>Task wrapped TResult</returns>
        public static async Task<TResult> WrapGrainCall<T1, T2, TResult>(Func<T1, T2, Task<TResult>> func, T1 param1, T2 param2, int timesToRetry)
        {
            if (timesToRetry < 0)
            {
                throw new OrleansGrainFailedAfterRetries();
            }

            timesToRetry--;

            try
            {
                return await func(param1, param2);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                return await WrapGrainCall(func, param1, param2, timesToRetry);
            }
        }

        /// <summary>
        /// Wrap a grain call that returns a <see cref="TResult"/> and takes three parameters.
        /// </summary>
        /// <typeparam name="T1">The first parameter type.</typeparam>
        /// <typeparam name="T2">The second parameter type.</typeparam>
        /// <typeparam name="T3">The third parameter type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="param1">The first parameter to use with the function invoke.</param>
        /// <param name="param2">The second parameter to use with the function invoke.</param>
        /// <param name="param3">The third parameter to use with the function invoke.</param>
        /// <param name="timesToRetry">The number of times to retry the grain call's that failed due to LoadShedding</param>
        /// <returns>Task wrapped TResult</returns>
        public static async Task<TResult> WrapGrainCall<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> func, T1 param1, T2 param2, T3 param3, int timesToRetry)
        {
            if (timesToRetry < 0)
            {
                throw new OrleansGrainFailedAfterRetries();
            }

            timesToRetry--;

            try
            {
                return await func(param1, param2, param3);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                return await WrapGrainCall(func, param1, param2, param3, timesToRetry);
            }
        }
    }
}
