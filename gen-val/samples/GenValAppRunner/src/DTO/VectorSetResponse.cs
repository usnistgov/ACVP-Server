 using System.Collections.Generic;
 using NIST.CVP.ACVTS.Libraries.Common.Enums;
    
namespace GenValAppRunner.DTO
 {
     public class VectorSetResponse
     {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public TestVectorSet Result { get; set; }
      }
 }   