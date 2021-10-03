using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Repositories;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Abstractions;

namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Repositories
{
    public class SupplyItemRepository: ISupplyItemRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private const string TableName = "supply_item";
        public SupplyItemRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task AddItems(ICollection<SupplyItem> items, CancellationToken cancellationToken)
        {
            var connection = await _dbConnectionFactory.GetConnectionAsync(cancellationToken);
            await connection.ExecuteAsync($@"
                INSERT INTO {TableName} (supply_id, sku, quantity)
                VALUES (@SupplyID, @Sku, @Quantity)", items);
        }

        public async Task<IEnumerable<SupplyItem>> GetItems(ICollection<long> supplyIds, CancellationToken 
            cancellationToken)
        {
            var connection = await _dbConnectionFactory.GetConnectionAsync(cancellationToken);
            return await connection.QueryAsync<SupplyItem>($@"
                SELECT sku, quantity, supply_id as SupplyId FROM {TableName}
                WHERE supply_id = any(@supplyIds)", new
            {
                supplyIds
            });
        }
    }
}