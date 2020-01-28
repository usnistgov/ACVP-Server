using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Pools.Models
{
    public class PoolOrleansJob
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public TimeSpan Duration => EndDate - StartDate;

        public IParameters PoolQueued { get; }
        public int JobsQueued { get; }

        public PoolOrleansJob(DateTime start, DateTime end, IParameters poolQueued, int jobsQueued)
        {
            StartDate = start;
            EndDate = end;
            PoolQueued = poolQueued;
            JobsQueued = jobsQueued;
        }
    }
}
