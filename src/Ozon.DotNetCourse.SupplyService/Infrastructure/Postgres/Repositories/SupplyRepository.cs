using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Repositories;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Abstractions;

namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Repositories
{
    public class SupplyRepository: ISupplyRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private const string TableName = "supply";

        public SupplyRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<long> Add(Supply supply, CancellationToken cancellationToken)
        {
            var connection = await _dbConnectionFactory.GetConnectionAsync(cancellationToken);
            var id = await connection.QuerySingleAsync<long>(@$"
                INSERT INTO {TableName} 
                    (created_at, state)
                VALUES (@CreatedAt, @State)
                RETURNING id", new
            {
                supply.CreatedAt,
                supply.State
            });
            return id;
        }

        public async Task<IEnumerable<Supply>> Get(SupplyState state, CancellationToken cancellationToken)
        {
            var connection = await _dbConnectionFactory.GetConnectionAsync(cancellationToken);
            return await connection.QueryAsync<Supply>(@$"
                SELECT id, state, created_at as CreatedAt FROM {TableName} 
                WHERE state = @state", new
            {
               state
            });
        }

        public async Task<IEnumerable<Supply>> Get(SupplyState state, DateTimeOffset createdTo, CancellationToken 
        cancellationToken)
        {
            var connection = await _dbConnectionFactory.GetConnectionAsync(cancellationToken);
            return await connection.QueryAsync<Supply>(@$"
                SELECT id, state, created_at as CreatedAt FROM {TableName} 
                WHERE state = @state
                AND created_at<=@createdTo", new
            {
                state, createdTo
            });
            
        }

        public async Task UpdateState(Supply supply, CancellationToken cancellationToken)
        {
            var connection = await _dbConnectionFactory.GetConnectionAsync(cancellationToken);
            await connection.QueryAsync<Supply>(@$"
                UPDATE {TableName}
                SET state = @State
                WHERE id = @Id", supply);
        }
    }
}