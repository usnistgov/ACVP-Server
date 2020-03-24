using System;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;
using Web.Public.Results;

namespace Web.Public.Providers
{
    public class VendorProvider : IVendorProvider
    {
        private readonly string _connectionString;
        
        public VendorProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public List<Organization> GetVendorList()
        {
            var db = new MightyOrm(_connectionString, "val.ORGANIZATION");

            try
            {
                var data = db.All(columns: "id, name");

                if (data == null)
                {
                    throw new Exception("Unable to find vendors");
                }

                return data.Select(v => new Organization {ID = v.id, Name = v.name}).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Unable to connect to database and find vendors", ex);
                throw;
            }
        }

        public Organization GetVendor(int id)
        {
            var db = new MightyOrm(_connectionString, "val.ORGANIZATION");

            try
            {
                var vendorData = db.Single(id);

                if (vendorData == null)
                {
                    throw new Exception($"Unable to find vendor with ID: {id}");
                }
                
                // Need to build other aspects of vendor
                var orderedEmails = GetEmailsByVendorId(id);

                return null;
                
            }
            catch (Exception ex)
            {
                Log.Error("Unable to connect to database and find vendor", ex);
                throw;
            }
        }

        public List<string> GetEmailsByVendorId(int vendorId)
        {
            var db = new MightyOrm(_connectionString, "val.ORGANIZATION_EMAIL");

            try
            {
                var emailData = db.All(new
                {
                    Organization_ID = vendorId
                }, "order_index");

                return emailData.Select(e => new string(e.email_address)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Unable to connect to database and find emails", ex);
                throw;
            }
        }

        public List<string> GetContactsByVendorId(int vendorId)
        {
            // db = new MightyOrm(_connectionString, "val.PERSON");
            // var contactData = db.All(new
            // {
            //     Org_ID = id
            // }, "order_index");
            return null;
        } 

        public VendorResult CreateVendor(Organization vendor)
        {
            if (vendor.Name == default)
            {
                return new VendorResult("Unable to create vendor with no name");
            }
            
            throw new System.NotImplementedException();
        }
        
        public VendorResult UpdateVendor(int id, Organization vendor)
        {
            throw new System.NotImplementedException();
        }

        public Result DeleteVendor(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}