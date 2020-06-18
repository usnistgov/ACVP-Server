using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.DatabaseInterface;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class PropertyProvider : IPropertyProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<PropertyProvider> _logger;
		private List<PropertyLookup> _properties;

		public PropertyProvider(IConnectionStringFactory connectionStringFactory, ILogger<PropertyProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
			_properties = GetProperties();
		}

		public List<PropertyLookup> GetProperties()
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<PropertyLookup> properties = new List<PropertyLookup>();

			try
			{
				var data = db.QueryFromProcedure("dbo.PropertiesGet");

				foreach (var property in data)
				{
					properties.Add(new PropertyLookup
					{
						AlgorithmID = property.AlgorithmId,
						PropertyID = property.AlgorithmPropertyId,
						Name = property.PropertyName,
						OrderIndex = property.OrderIndex
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
			}

			return properties;
		}

		public List<PropertyTreeValidationNode> GetPropertyTreeValidationNodes(long algorithmID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<PropertyTreeValidationNode> nodes = new List<PropertyTreeValidationNode>();

			try
			{
				var data = db.QueryFromProcedure("dbo.PropertyTreeValidationNodesGet", inParams: new { AlgorithmId = algorithmID });

				foreach (var item in data)
				{
					nodes.Add(new PropertyTreeValidationNode
					{
						ID = item.PropertyID,
						Name = item.PropertyName,
						Level = item.Level,
						OrderIndex = item.OrderIndex,
						Type = (AlgorithmPropertyType)item.AlgorithmPropertyTypeId
					});
				}
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.ToString());
			}

			return nodes;
		}
	}
}
