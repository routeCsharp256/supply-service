using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Notifiers;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Repositories;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services;

namespace Ozon.DotNetCourse.SupplyService.Services
{
    public class SupplyService: ISupplyService
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly ISupplyItemRepository _supplyItemRepository;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ISupplyShippedNotifier _supplyShippedNotifier;

        public SupplyService(ISupplyRepository supplyRepository, ISupplyItemRepository supplyItemRepository, ITransactionFactory transactionFactory, ISupplyShippedNotifier supplyShippedNotifier)
        {
            _supplyRepository = supplyRepository;
            _supplyItemRepository = supplyItemRepository;
            _transactionFactory = transactionFactory;
            _supplyShippedNotifier = supplyShippedNotifier;
        }

        public async Task Create(Supply supply, CancellationToken cancellationToken)
        {
            using var transaction = _transactionFactory.CreateTransactionScope();
            var supplyId = await _supplyRepository.Add(supply, cancellationToken);
            supply.Id = supplyId;
            await _supplyItemRepository.AddItems(supply.Items, cancellationToken);
            transaction.Complete();
        }

        public async Task<IEnumerable<Supply>> Get(SupplyState state, CancellationToken cancellationToken)
        {
            var supplies = (await _supplyRepository.Get(state, cancellationToken)).ToList();
            var supplyItems =
                (await _supplyItemRepository.GetItems(supplies.Select(x => x.Id).ToList(), cancellationToken))
                .GroupBy(x => x.SupplyId).ToDictionary(x => x.Key);
            
            foreach (var supply in supplies)
            {
                supply.Items = supplyItems.GetValueOrDefault(supply.Id)?.ToList() ?? new List<SupplyItem>();
            }

            return supplies;
        }
        
        public async Task<IEnumerable<Supply>> Get(SupplyState state, DateTimeOffset createdTo, CancellationToken cancellationToken)
        {
            var supplies = (await _supplyRepository.Get(state, createdTo, cancellationToken)).ToList();
            var supplyItems =
                (await _supplyItemRepository.GetItems(supplies.Select(x => x.Id).ToList(), cancellationToken))
                .GroupBy(x => x.SupplyId).ToDictionary(x => x.Key);
            
            foreach (var supply in supplies)
            {
                supply.Items = supplyItems.GetValueOrDefault(supply.Id)?.ToList() ?? new List<SupplyItem>();
            }

            return supplies;
        }

        public async Task Ship(Supply supply,CancellationToken cancellationToken)
        {
            supply.State = SupplyState.Shipped;
            using var transaction = _transactionFactory.CreateTransactionScope();

            await _supplyRepository.UpdateState(supply, cancellationToken);
            await _supplyShippedNotifier.Notify(supply, cancellationToken);
        }
    }
}