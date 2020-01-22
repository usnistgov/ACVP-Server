using System;
using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class TestSession : TestSessionLite
    {
        public DateTime? PassedOn { get; set; }
        public bool Publishable { get; set; }
        public bool Published { get; set; }
        public bool IsSample { get; set; }
        public bool HasCapabilitiesFile { get; set; }
        public bool HasPromptFile { get; set; }
        public bool HasInternalProjectionFile { get; set; }
        public bool HasExpectedAnswersFile { get; set; }
        public bool HasSubmittedAnswersFile { get; set; }
        public bool HasValidationResultsFile { get; set; }
        public bool HasErrorFile { get; set; }
        public List<TestVectorSetLite> VectorSets { get; set; } = new List<TestVectorSetLite>();
    }
}