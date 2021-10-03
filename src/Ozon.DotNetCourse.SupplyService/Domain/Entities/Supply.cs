using System;
using System.Collections;
using System.Collections.Generic;

namespace Ozon.DotNetCourse.SupplyService.Domain.Entities
{
    public class Supply
    {
        public static Supply Create(ICollection<SupplyItem> items)
        {
            return new Supply
            {
                State = SupplyState.Created,
                CreatedAt = DateTimeOffset.UtcNow,
                Items = items
            };
        }

        private Supply()
        {
            Items = new List<SupplyItem>();
        }

        private long _id;

        public long Id
        {
            get => _id;
            set
            {
                if (_id > 0)
                {
                    throw new InvalidOperationException($"{nameof(Id)} already setted");
                }

                _id = value;
                foreach (var supplyItem in Items)
                {
                    supplyItem.SupplyId = _id;
                }
            }
        }

        public SupplyState State { get; set; }
        
        public DateTimeOffset CreatedAt { get; private set; }
        
        public ICollection<SupplyItem> Items { get; set; }
    }

    public enum SupplyState
    {
        Created = 1,
        Shipped = 2,
    }
}