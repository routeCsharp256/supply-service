using System.Threading;
using System.Threading.Tasks;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;

namespace Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Notifiers
{
    public interface ISupplyShippedNotifier
    {
        public Task Notify(Supply supply, CancellationToken cancellationToken);
    }
}