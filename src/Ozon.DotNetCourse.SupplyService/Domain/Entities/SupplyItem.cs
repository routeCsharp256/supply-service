using System;

namespace Ozon.DotNetCourse.SupplyService.Domain.Entities
{
    public class SupplyItem
    {
        public SupplyItem()
        {
            
        }
        public SupplyItem(long sku, int quantity)
        {
            Sku = sku;
            Quantity = quantity;
        }

        private long _supplyId;
        public long SupplyId
        {
            get => _supplyId;
            set
            {
                if (_supplyId > 0)
                {
                    throw new InvalidOperationException($"{nameof(SupplyId)} already setted");
                }

                _supplyId = value;
            }
        }

        public long Sku { get; private set; }
        public int Quantity { get; private set; }
    }
}