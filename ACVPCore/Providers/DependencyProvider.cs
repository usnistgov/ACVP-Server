using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class DependencyProvider : IDependencyProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<DependencyProvider> _logger;

		public DependencyProvider(IConnectionStringFactory connectionStringFactory, ILogger<DependencyProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Dependency Get(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.DependencyGet", inParams: new
				{
					DependencyID = dependencyID
				});

				if (data != null)
				{
					return new Dependency
					{
						ID = dependencyID,
						Name = data.Name,
						Type = data.Type,
						Description = data.Description,
						Attributes = GetAttributes(dependencyID)	//TODO - decide whether or not this should be populated, or if I just want the base dependency object
					};
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		public List<DependencyAttribute> GetAttributes(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<DependencyAttribute> dependencyAttributes = new List<DependencyAttribute>();
			try
			{
				var data = db.QueryFromProcedure("val.DependencyAttributesGet", inParams: new
				{
					DependencyID = dependencyID
				});

				foreach (var attribute in data)
				{
					dependencyAttributes.Add(new DependencyAttribute
					{
						ID = attribute.ID,
						DependencyID = dependencyID,
						Name = attribute.Name,
						Value = attribute.Value
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return dependencyAttributes;
		}

		public Result Delete(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.DependencyDelete @0", dependencyID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllAttributes(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.DependencyAttributeDeleteAll @0", dependencyID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAttribute(long attributeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.DependencyAttributeDelete @0", attributeID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllOELinks(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.DependencyOELinkForDependencyDeleteAll @0", dependencyID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteOELink(long dependencyID, long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.DependencyOELinkDelete @0, @1", dependencyID, oeID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(string type, string name, string description)
		{
			if (string.IsNullOrWhiteSpace(type)) return new InsertResult("Invalid type value");
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.DependencyInsert", inParams: new
				{
					Type = type,
					Name = name,
					Description = description
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert dependency");
				}
				else
				{
					return new InsertResult((long)data.DependencyID);
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public InsertResult InsertAttribute(long dependencyID, string name, string value)
		{
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");
			if (string.IsNullOrWhiteSpace(value)) return new InsertResult("Invalid value value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.DependencyAttributeInsert", inParams: new
				{
					DependencyID = dependencyID,
					Name = name,
					Value = value
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert dependency attribute");
				}
				else
				{
					return new InsertResult((long)data.DependencyAttributeID);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result Update(long dependencyID, string type, string name, string description, bool typeUpdated, bool nameUpdated, bool descriptionUpdated)
		{
			if (typeUpdated && string.IsNullOrWhiteSpace(type)) return new Result("Invalid type value");
			if (nameUpdated && string.IsNullOrWhiteSpace(name)) return new Result("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("val.DependencyUpdate", inParams: new
				{
					DependencyID = dependencyID,
					Type = type,
					Name = name,
					Description = description,
					TypeUpdated = typeUpdated,
					NameUpdated = nameUpdated,
					DescriptionUpdated = descriptionUpdated
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public bool DependencyIsUsed(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.DependencyIsUsed", inParams: new
				{
					DependencyID = dependencyID
				});

				return data.IsUsed;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}
	}
}
