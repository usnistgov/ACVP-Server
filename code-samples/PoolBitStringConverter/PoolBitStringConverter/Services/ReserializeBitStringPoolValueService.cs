using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Interfaces;
using PoolBitStringConverter.Models;
using System;
using System.Collections.Generic;

namespace PoolBitStringConverter.Services
{
    internal class ReserializeBitStringPoolValueService
    {
        /// <summary>
        /// Number of pool value records to work with at a time.
        /// </summary>
        private const int ChunkSize = 1000;

        private readonly IServiceProvider _serviceProvider;
        private readonly IDbConnectionStringFactory _dbConnectionStringFactory;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ReserializeBitStringPoolValueService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbConnectionStringFactory = _serviceProvider.GetService<IDbConnectionStringFactory>();
            _dbConnectionFactory = serviceProvider.GetService<IDbConnectionFactory>();
        }

        public void Execute()
        {
            var poolTypes = GetPoolTypes();
            UpdatePoolValuesPerPoolType(poolTypes);
        }

        /// <summary>
        /// Get each pool type that exists and return as a <see cref="List{T}" /> of <see cref="PoolType"/>.
        /// </summary>
        /// <returns></returns>
        private List<PoolType> GetPoolTypes()
        {
            return new PoolInformationSqlRepository(_dbConnectionStringFactory, _dbConnectionFactory)
                .GetPoolTypes();
        }

        /// <summary>
        /// For every pool type, we want to update the values for that type.
        /// </summary>
        /// <param name="poolTypes">The poolTypes to enumerate.</param>
        private void UpdatePoolValuesPerPoolType(IEnumerable<PoolType> poolTypes)
        {
            foreach (var poolType in poolTypes)
            {
                UpdatePoolValues(poolType);
            }
        }

        /// <summary>
        /// Update an individual poolType's values
        /// </summary>
        /// <param name="poolType"></param>
        private void UpdatePoolValues(PoolType poolType)
        {
            var poolValues = GetValuesForPool(poolType);
            SaveValuesForPool(poolType, poolValues);
        }

        private List<PoolValues> GetValuesForPool(PoolType poolType)
        {
            throw new System.NotImplementedException();
        }

        private void SaveValuesForPool(PoolType poolType, List<PoolValues> poolValues)
        {
            throw new System.NotImplementedException();
        }
    }
}