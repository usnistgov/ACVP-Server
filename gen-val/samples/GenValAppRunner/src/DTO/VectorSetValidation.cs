using System.Collections.Generic;

namespace GenValAppRunner.DTO
 {
    public class VectorSetValidationResults
    {
        public int VsId { get; set; }
        public string Disposition {get; set;}
        public List<TestCaseValidationResult> Tests { get; set; }

    }

    public class TestCaseValidationResult
    {
        public int TcId { get; set; }
        public string Result { get; set; }
    }

 }