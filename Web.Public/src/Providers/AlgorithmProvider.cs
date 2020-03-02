using System;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.External;
using ACVPCore.ExtensionMethods;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Options;
using Mighty;
using Serilog;
using Web.Public.Configs;

namespace Web.Public.Providers
{
    public class AlgorithmProvider : IAlgorithmProvider
    {
        private readonly string _acvpConnectionString;
        private readonly AlgorithmConfig _options;
        private IEnumerable<AlgorithmBase> _cachedAlgorithmList = new List<AlgorithmBase>();

        public AlgorithmProvider(IConnectionStringFactory connectionStringFactory, IOptions<AlgorithmConfig> options)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
            _options = options.Value;
        }
        
        public IEnumerable<AlgorithmBase> GetAlgorithmList()
        {
            if (_cachedAlgorithmList.Any() && _options.AllowCaching)
            {
                return _cachedAlgorithmList;
            }
            
            var db = new MightyOrm<AlgorithmBase>(_acvpConnectionString);

            try
            {
                var algoList = db.QueryWithExpando("ref.AlgorithmsGet");
                _cachedAlgorithmList = algoList.Data;
                return _cachedAlgorithmList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw ex;
            }
        }
    }
}