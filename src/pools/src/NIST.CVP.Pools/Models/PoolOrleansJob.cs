using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Pools.Models
{
    public class PoolOrleansJob
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public TimeSpan Duration => EndDate - StartDate;

        public string PoolQueued { get; }
        public int JobsQueued { get; }

        public PoolOrleansJob(DateTime start, DateTime end, string poolQueued, int jobsQueued)
        {
            StartDate = start;
            EndDate = end;
            PoolQueued = poolQueued;
            JobsQueued = jobsQueued;
        }
    }
}
