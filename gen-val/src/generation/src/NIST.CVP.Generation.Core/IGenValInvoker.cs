using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Used to invoke a specific set of GenVals from a request. 
    /// </summary>
    public interface IGenValInvoker
    {
        /// <summary>
        /// Run the parameter validator for the provided registration request.
        /// </summary>
        /// <param name="request">The algorithm capabilities json.</param>
        /// <returns></returns>
        ParameterCheckResponse CheckParameters(ParameterCheckRequest request);
        /// <summary>
        /// Run the generator for the registration request.
        /// </summary>
        /// <param name="request">The algorithm capabilities json.</param>
        /// <returns></returns>
        Task<GenerateResponse> GenerateAsync(GenerateRequest request, long vsId);
        /// <summary>
        /// Run the validator given the ACVP internal projection and IUT responses.
        /// </summary>
        /// <param name="request">The ACVP internal projection and IUT json.</param>
        /// <returns></returns>
        Task<ValidateResponse> ValidateAsync(ValidateRequest request, long vsId);
    }
}