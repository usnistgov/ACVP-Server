using System;
using System.Collections.Generic;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public class AddressProvider : IAddressProvider
    {
        private readonly string _connectionString;
        
        public AddressProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public Address Get(long vendorId, long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var addressData = db.SingleFromProcedure("val.AddressGet", inParams: new
                {
                    AddressID = id
                });

                if (addressData == null)
                {
                    throw new Exception("Unable to find address");
                }
                
                var result = new Address
                {
                    ID = addressData.ID,
                    OrganizationID = addressData.OrganizationID,
                    Street1 = addressData.Street1,
                    Street2 = addressData.Street2,
                    Street3 = addressData.Street3,
                    Locality = addressData.Locality,
                    Region = addressData.Region,
                    Country = addressData.Country,
                    PostalCode = addressData.PostalCode
                };

                return result.OrganizationID == vendorId ? result : null;
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to find address: {id}", ex);
                throw;
            }
        }

        public (long TotalCount, List<Address> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter)
        {
            throw new System.NotImplementedException();
        }
    }
}