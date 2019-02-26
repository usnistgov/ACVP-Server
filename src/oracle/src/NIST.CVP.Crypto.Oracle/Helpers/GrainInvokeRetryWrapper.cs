using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Crypto.Oracle.Helpers
{
    public static class GrainInvokeRetryWrapper
    {
        public static async Task WrapGrainCall<T>(Func<T, Task> func, T param)
        {
            try
            {
                await func(param);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                await WrapGrainCall(func, param);
            }
        }

        public static async Task<TResult> WrapGrainCall<T, TResult>(Func<T, Task<TResult>> func, T param)
        {
            try
            {
                return await func(param);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                return await WrapGrainCall(func, param);
            }
        }

        public static async Task<TResult> WrapGrainCall<T1, T2, TResult>(Func<T1, T2, Task<TResult>> func, T1 param1, T2 param2)
        {
            try
            {
                return await func(param1, param2);
            }
            catch (GatewayTooBusyException)
            {
                // Gateway too busy - wait 5 seconds and try again.
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                return await WrapGrainCall(func, param1, param2);
            }
        }
    }
}
