namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Kafka
{
    public class Configuration
    {
        public string BootstrapServers { get; set; }
        public string SupplyShippedTopicName { get; set; }
    }
}