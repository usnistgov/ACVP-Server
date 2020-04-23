using System;
using System.Collections.Generic;
using System.Linq;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
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
                    return null;
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

        public List<Address> GetAddressList(long vendorId)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var addressData = db.QueryFromProcedure("val.AddressesForOrganizationGet", inParams: new
                {
                    OrganizationID = vendorId
                });

                if (addressData == null)
                {
                    throw new Exception("Unable to find address");
                }
                
                var addresses = addressData.ToList().Select(address => new Address
                    {
                        ID = address.ID,
                        OrganizationID = address.OrganizationID,
                        Street1 = address.Street1,
                        Street2 = address.Street2,
                        Street3 = address.Street3,
                        Locality = address.Locality,
                        Region = address.Region,
                        Country = address.Country,
                        PostalCode = address.PostalCode
                    })
                    .ToList();

                return addresses;
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to find addresses for organization: {vendorId}", ex);
                throw;
            }
        }
    }
}