using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using System.Numerics;

namespace NIST.CVP.Generation.TDES_ECB
{
    

    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };



        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
           
            ValidateDirection(parameters, errorResults);
           

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

     

        private void ValidateDirection(Parameters parameters,  List<string> errorResults)
        {
            string result = ValidateArray(parameters.Mode, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
