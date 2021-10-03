using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;

namespace Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Repositories
{
    public interface ISupplyRepository
    {
        Task<long> Add(Supply supply, CancellationToken cancellationToken);
        
        Task<IEnumerable<Supply>> Get(SupplyState state, CancellationToken cancellationToken);
        
        Task<IEnumerable<Supply>> Get(SupplyState state, DateTimeOffset createdTo, CancellationToken cancellationToken);
        
        Task UpdateState(Supply supply, CancellationToken cancellationToken);
    }
}