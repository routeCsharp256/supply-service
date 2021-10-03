using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;

namespace Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Repositories
{
    public interface ISupplyItemRepository
    {
        Task AddItems(ICollection<SupplyItem> items, CancellationToken cancellationToken);
        Task<IEnumerable<SupplyItem>> GetItems(ICollection<long> supplyIds, CancellationToken cancellationToken);
    }
}