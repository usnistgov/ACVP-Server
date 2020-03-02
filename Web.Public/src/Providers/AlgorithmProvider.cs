using System;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.External;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;

namespace Web.Public.Providers
{
    public class AlgorithmProvider : IAlgorithmProvider
    {
        private readonly string _acvpConnectionString;
        private IEnumerable<AlgorithmBase> _cachedAlgorithmList = new List<AlgorithmBase>();

        public AlgorithmProvider(IConnectionStringFactory connectionStringFactory)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public IEnumerable<AlgorithmBase> GetAlgorithmList()
        {
            if (_cachedAlgorithmList.Any())
            {
                return _cachedAlgorithmList;
            }
            
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var algoList = db.ExecuteProcedure("ref.AlgorithmsGet");

                var x = 5;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw ex;
            }
            
            throw new System.NotImplementedException();
        }
    }
}