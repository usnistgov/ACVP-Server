using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    /// <summary>
    /// Interface for performing crypto operations.
    /// 
    /// NOTE: Since we'll likely be touching interface methods, 
    /// separating out into partial classes to help avoid merge conflicts.
    /// Classes are nested underneath this base in solution explorer
    /// </summary>
    public partial interface IOracle
    {
        Task InitializeClusterClient();
        Task CloseClusterClient();

        public bool CanRetrieveFromPools { get; }
    }
}
