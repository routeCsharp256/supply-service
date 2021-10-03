using System.Collections.Generic;

namespace Ozon.DotNetCourse.SupplyService.DTO.KafkaMessage
{
    public class SupplyShipped
    {
        public long SupplyId { get; set; }
        public ICollection<Item> Items { get; set; }
        public record Item(long SkuId, int Quantity);
    }
}