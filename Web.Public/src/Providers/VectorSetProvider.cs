using System;
using System.Text;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Serilog;
using Web.Public.Models;
using VectorSetStatus = Web.Public.Models.VectorSetStatus;

namespace Web.Public.Providers
{
    public class VectorSetProvider : IVectorSetProvider
    {
        private readonly string _connectionString;
        
        public VectorSetProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public long GetNextVectorSetID(long tsID, string token)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var nextID = db.SingleFromProcedure("external.VectorSetGetNextID", new
                {
                    TestSessionID = tsID
                });

                if (nextID == null)
                {
                    throw new Exception("Unable to get next ID");
                }

                return (long)nextID.ID;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving next VectorSet ID");
                throw;
            }
        }

        public VectorSetStatus CheckStatus(long vsID)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var statusData = db.SingleFromProcedure("acvp.VectorSetStatusGet", new
                {
                    VsID = vsID
                });

                if (statusData == null)
                {
                    // Vector set does not exist
                    throw new Exception("Vector set does not exist");
                }

                return (VectorSetStatus)statusData.Status;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error retrieving VectorSet status: {vsID}");
                throw;
            }
        }

        public VectorSet GetJson(long vsID, VectorSetJsonFileTypes fileType)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var jsonData = db.SingleFromProcedure("acvp.VectorSetJsonGet", new
                {
                    VsID = vsID,
                    FileType = (int)fileType
                });

                if (jsonData == null)
                {
                    // Prep for retry
                    return null;
                }

                // For VECTOR_SET_DATA
                var contentBytes = (byte[])jsonData.Content;
                var contentJson = Encoding.UTF8.GetString(contentBytes);
                return new VectorSet
                {
                    JsonContent = contentJson
                };

                // For VectorSetJson
                //return new VectorSet
                //{
                //    JsonContent = jsonData.Content
                //};
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error retrieving vector set file. VsID: {vsID}, FileType: {fileType}");
                throw;
            }
        }
    }
}