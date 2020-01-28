using NIST.CVP.Pools.Enums;
using System;

namespace NIST.CVP.Pools.Interfaces
{
    public interface IPoolLogRepository
    {
        void WriteLog(LogTypes logType, string poolName, DateTime dateStart, DateTime? dateEnd, string msg);
    }
}