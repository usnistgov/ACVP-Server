using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        // NOTE: Since we'll likely be touching interface methods, 
        // separating out into partial classes to help avoid merge conflicts.
        // Classes are nested underneath this base in solution explorer
        Task InitializeClusterClient();
        Task CloseClusterClient();
    }
}
