﻿using System;
using System.Collections.Generic;
using ACVPCore.Models;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
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
				var data = db.QueryFromProcedure("ref.PropertiesGet");

				foreach (var property in data)
				{
					properties.Add(new PropertyLookup
					{
						AlgorithmID = property.AlgorithmId,
						PropertyID = property.PropertyId,
						Name = property.Name,
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
	}
}