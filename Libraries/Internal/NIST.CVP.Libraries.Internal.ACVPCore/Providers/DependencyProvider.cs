using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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
				var data = db.SingleFromProcedure("dbo.DependencyGet", inParams: new
				{
					DependencyId = dependencyID
				});

				if (data != null)
				{
					return new Dependency
					{
						ID = dependencyID,
						Name = data.Name,
						Type = data.DependencyType,
						Description = data.Description,
						Attributes = GetAttributes(dependencyID)    //TODO - decide whether or not this should be populated, or if I just want the base dependency object
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
				var data = db.QueryFromProcedure("dbo.DependencyAttributesGet", inParams: new
				{
					DependencyID = dependencyID
				});

				foreach (var attribute in data)
				{
					dependencyAttributes.Add(new DependencyAttribute
					{
						ID = attribute.DependencyAttributeId,
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
				db.ExecuteProcedure("dbo.DependencyDelete", inParams: new { DependencyId = dependencyID });
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
				db.ExecuteProcedure("dbo.DependencyAttributeDeleteAll", inParams: new { DependencyId = dependencyID });
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
				db.ExecuteProcedure("dbo.DependencyAttributeDelete", inParams: new { DependencyAttributeId = attributeID });
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
				db.ExecuteProcedure("dbo.OEDependencyForDependencyDeleteAll", inParams: new { DependencyId = dependencyID });
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
				db.ExecuteProcedure("dbo.OEDependencyDelete", inParams: new { DependencyId = dependencyID, OEId = oeID });
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
				var data = db.SingleFromProcedure("dbo.DependencyInsert", inParams: new
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
					return new InsertResult((long)data.DependencyId);
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
				var data = db.SingleFromProcedure("dbo.DependencyAttributeInsert", inParams: new
				{
					DependencyId = dependencyID,
					Name = name,
					Value = value
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert dependency attribute");
				}
				else
				{
					return new InsertResult((long)data.DependencyAttributeId);
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
				db.ExecuteProcedure("dbo.DependencyUpdate", inParams: new
				{
					DependencyId = dependencyID,
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
				return (bool)db.ScalarFromProcedure("dbo.DependencyIsUsed", inParams: new
				{
					DependencyId = dependencyID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public bool DependencyExists(long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.DependencyExists", inParams: new
				{
					DependencyId = dependencyID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}

		public PagedEnumerable<Dependency> Get(DependencyListParameters param)
		{
			var result = new List<Dependency>();
			long totalRecords = 0;
			var db = new MightyOrm<Dependency>(_acvpConnectionString);

			try
			{
				var dbResult = db.QueryWithExpando("dbo.DependenciesGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					DependencyId = param.Id,
					Name = param.Name,
					Type = param.Type,
					Description = param.Description
				}, new
				{
					totalRecords = (long)0
				});

				result = dbResult.Data;
				totalRecords = dbResult.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return result.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}
	}
}
