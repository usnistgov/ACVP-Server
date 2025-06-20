using System.Collections.Generic;

namespace GenValAppRunner.DTO
 {
    public class VectorSetValidationResults
    {
        public int VsId { get; set; }
        public string Disposition {get; set;}
        public List<TestCaseValidation> Tests { get; set; }

    }
    public class TestCaseValidation
    {
        public int TcId { get; set; }
        public string Results { get; set; }
    }

 }