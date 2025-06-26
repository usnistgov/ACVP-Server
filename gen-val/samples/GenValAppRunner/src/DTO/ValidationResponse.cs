using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.Enums;

namespace GenValAppRunner.DTO
 {
      public class ValidationResponse
      {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public VectorSetValidationResults Result { get; set; }
      }
 }