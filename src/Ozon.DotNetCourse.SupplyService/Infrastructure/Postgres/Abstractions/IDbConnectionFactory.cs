using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Abstractions
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken);
    }
}