﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Pools.Models
{
    public class SpawnJobResponse
    {
        public bool HasSpawnedJob { get; set; }
        public IParameters PoolParameter { get; set; }
    }
}