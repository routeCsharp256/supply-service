using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;

namespace Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services
{
    public interface ISupplyService
    {
        Task Create(Supply supply, CancellationToken cancellationToken);
        Task<IEnumerable<Supply>> Get(SupplyState state, CancellationToken cancellationToken);

        Task Ship(Supply supply, CancellationToken cancellationToken);

        Task<IEnumerable<Supply>> Get(SupplyState state, DateTimeOffset createdTo, CancellationToken
            cancellationToken);
    }
}