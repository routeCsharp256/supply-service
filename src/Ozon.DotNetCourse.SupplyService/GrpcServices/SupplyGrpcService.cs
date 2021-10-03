using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services;
using Ozon.DotNetCourse.SupplyService.GRPC;

namespace Ozon.DotNetCourse.SupplyService.GrpcServices
{
    public class SupplyGrpcService: GRPC.SupplyService.SupplyServiceBase
    {
        private readonly ISupplyService _supplyService;

        public SupplyGrpcService(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        public override async Task<RequestSupplyResponse> RequestSupply(RequestSupplyRequest request, ServerCallContext 
        context)
        {
            if (!request.Items.Any())
            {
                throw new ArgumentException($"{nameof(RequestSupplyRequest.Items)} can not be empty");
            }

            var items = new List<SupplyItem>(request.Items.Count);

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                {
                    throw new ArgumentException(
                        $"{nameof(RequestSupplyRequest.Types.SupplyItem.Quantity)} must be greater than zero");
                }
                
                if (item.Sku <= 0)
                {
                    throw new ArgumentException(
                        $"{nameof(RequestSupplyRequest.Types.SupplyItem.Sku)} must be greater than zero");
                }
                
                items.Add(new SupplyItem(item.Sku, item.Quantity));
            }

            var supply = Supply.Create(items);

            await _supplyService.Create(supply, context.CancellationToken);
            return new RequestSupplyResponse
            {
                SupplyId = supply.Id
            };
        }
    }
}