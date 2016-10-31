using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    public interface ITestVectorSet
    {
        string Algorithm { get; set; }
        bool IsSample { get; set; }
        List<ITestGroup> TestGroups { get; set; }
        List<dynamic> AnswerProjection { get; }
        List<ITestGroup> PromptProjection { get; }
        List<ITestResult> ResultProjection { get; }

    }
}
