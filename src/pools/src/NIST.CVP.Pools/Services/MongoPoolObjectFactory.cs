using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Services
{
    public class MongoPoolObjectFactory : IMongoPoolObjectFactory
    {
        public MongoPoolObject<TResult> WrapResult<TResult>(TResult result) where TResult : IResult
        {
            return new MongoPoolObject<TResult>()
            {
                Value = result
            };
        }
    }
}