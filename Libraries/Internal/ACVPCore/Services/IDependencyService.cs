﻿using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace ACVPCore.Services
{
	public interface IDependencyService
	{
		InsertResult InsertAttribute(long dependencyID, string name, string value);
		DeleteResult Delete(long dependencyID);
		Result DeleteAttribute(long attributeID);
		DeleteResult DeleteEvenIfUsed(long dependencyID);
		DependencyResult Create(DependencyCreateParameters dependency);
		DependencyResult Update(DependencyUpdateParameters parameters);
		Dependency Get(long dependencyId);
		PagedEnumerable<Dependency> Get(DependencyListParameters param);
		bool DependencyIsUsed(long dependencyID);
		bool DependencyExists(long dependencyID);
	}
}