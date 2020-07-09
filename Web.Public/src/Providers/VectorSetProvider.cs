using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class VectorSetProvider : IVectorSetProvider
    {
        private readonly ILogger<VectorSetProvider> _logger;
        private readonly string _connectionString;
        
        public VectorSetProvider(ILogger<VectorSetProvider> logger, IConnectionStringFactory connectionStringFactory)
        {
            _logger = logger;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public long GetNextVectorSetID(long tsID, string token)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var nextID = db.SingleFromProcedure("dbo.VectorSetGetNextID", new
                {
                    TestSessionId = tsID
                });

                if (nextID == null)
                {
                    throw new Exception("Unable to get next ID");
                }

                return (long)nextID.VectorSetId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving next VectorSet ID");
                throw;
            }
        }

        public VectorSetStatus GetStatus(long vsID)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var statusData = db.SingleFromProcedure("dbo.VectorSetStatusGet", new
                {
                    VectorSetId = vsID
                });

                if (statusData == null)
                {
                    // Vector set does not exist
                    return VectorSetStatus.Initial;
                }

                return (VectorSetStatus)statusData.VectorSetStatusId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving VectorSet status: {vsID}");
                throw;
            }
        }

        public VectorSet GetJson(long vsID, VectorSetJsonFileTypes fileType)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var jsonData = db.SingleFromProcedure("dbo.VectorSetJsonGet", new
                {
                    VectorSetId = vsID,
                    VectorSetJsonFileTypeId = (int)fileType
                }, commandTimeout: 120);

                if (jsonData == null)
                {
                    // Prep for retry
                    return null;
                }

                return new VectorSet
                {
                    JsonContent = jsonData.Content
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving vector set file. VsID: {vsID}, FileType: {fileType}");
                throw;
            }
        }

        public async Task<VectorSet> GetJsonAsync(long vsID, VectorSetJsonFileTypes fileType)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var jsonData = await db.SingleFromProcedureAsync("acvp.VectorSetJsonGet", new
                {
                    VectorSetId = vsID,
                    VectorSetJsonFileTypeId = (int)fileType
                }, commandTimeout: 120);

                if (jsonData == null)
                {
                    // Prep for retry
                    return null;
                }

                return new VectorSet
                {
                    JsonContent = jsonData.Content
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving vector set file. VsID: {vsID}, FileType: {fileType}");
                throw;
            }
        }

        public void SetStatus(long vsID, VectorSetStatus status)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                db.ExecuteProcedure("dbo.VectorSetStatusSet", new
                {
                    VectorSetId = vsID,
                    VectorSetStatusId = (int)status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error setting vector set status {vsID}");
                throw;
            }
        }
    }
}