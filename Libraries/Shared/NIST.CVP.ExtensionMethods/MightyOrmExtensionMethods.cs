using System.Collections.Generic;
using System.Linq;
using Mighty;

namespace NIST.CVP.ExtensionMethods
{
    public static class MightyOrmExtensionMethods
    {
        public static MightyResultsWithExpando<T> QueryWithExpando<T>(this MightyOrm<T> db,
            string sql, object inParams = null, object outParams = null, bool isProcedure = true)
            where T : class, new()
        {
            var items = new List<T>();
            dynamic resultsExpando;

            using (var cmd = db.CreateCommandWithParams(sql, inParams: inParams, outParams: outParams, isProcedure: isProcedure))
            {
                items = db.Query(cmd).ToList();
                resultsExpando = db.ResultsAsExpando(cmd);
            }
            
            return new MightyResultsWithExpando<T>(items, resultsExpando);
        }
    }
}