using System.Collections.Generic;

namespace GenValAppRunner.DTO
 {

    public class ValidationRequest
    {
        public TestVectorSet Answer {get;set;}
        public TestVectorSet Expected {get;set;}
    }
 }  