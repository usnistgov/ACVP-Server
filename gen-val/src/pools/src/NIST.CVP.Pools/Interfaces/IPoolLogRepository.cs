using NIST.CVP.Pools.Enums;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Pools.Interfaces
{
    public interface IPoolLogRepository
    {
        Task WriteLog(LogTypes logType, string poolName, DateTime dateStart, DateTime? dateEnd, string msg);
    }
}