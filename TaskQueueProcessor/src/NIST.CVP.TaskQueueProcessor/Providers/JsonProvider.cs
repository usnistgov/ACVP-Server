using System;
using CVP.DatabaseInterface;
using Mighty;
using NIST.CVP.TaskQueueProcessor.Constants;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public class JsonProvider : IJsonProvider
    {
        private readonly string _connectionString;
        
        public JsonProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
        }
        
        public string GetJson(int vsId, string jsonFileType)
        {
            var acvp = new MightyOrm(_connectionString);

            try
            {
                var jsonData = acvp.SingleFromProcedure(StoredProcedures.GET_JSON, new
                {
                    VsId = vsId,
                    JsonFileType = jsonFileType
                });

                return jsonData.Content;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unable to read JSON for vsId: {vsId}, jsonType: {jsonFileType}");
                throw;
            }
        }

        public void PutJson(int vsId, string jsonFileType, string json)
        {
            var acvp = new MightyOrm(_connectionString);

            try
            {
                acvp.ExecuteProcedure(StoredProcedures.PUT_JSON, new
                {
                    VsId = vsId,
                    JsonFileType = jsonFileType,
                    Content = json
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unable to write JSON for vsId: {vsId}, jsonType: {jsonFileType}, content: {json}");
                throw;
            }
        }
    }
}