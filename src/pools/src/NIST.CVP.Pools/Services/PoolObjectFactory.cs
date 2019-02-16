using System;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Services
{
    public class PoolObjectFactory : IPoolObjectFactory
    {
        public PoolObject<TResult> WrapResult<TResult>(TResult result) where TResult : IResult
        {
            return new PoolObject<TResult>()
            {
                Value = result,
                DateCreated = DateTime.Now
            };
        }

        public PoolObject<TResult> WrapResult<TResult>(
            TResult result, DateTime creationDate, DateTime lastUsedDate, int timesUsed
        )  where TResult : IResult
        {
            return new PoolObject<TResult>()
            {
                Value = result,
                DateCreated = creationDate,
                DateLastUsed = lastUsedDate,
                TimesUsed = timesUsed
            };
        }
    }
}