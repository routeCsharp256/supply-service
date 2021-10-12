using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Notifiers;
using Ozon.DotNetCourse.SupplyService.DTO.KafkaMessage;

namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Kafka
{
    public class SupplyShippedProducer: ISupplyShippedNotifier
    {
        
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new JsonConverter[] {new StringEnumConverter()}
        };
        
        private readonly IProducer<string, string> _producerWithKey;
        private readonly string _topicName;
        
        
        public SupplyShippedProducer(
            IOptions<Configuration> settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Value?.BootstrapServers))
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var config = new ClientConfig
            {
                BootstrapServers = settings.Value.BootstrapServers
            };

            _producerWithKey = new ProducerBuilder<string, string>(config).Build();
            _topicName = settings.Value.SupplyShippedTopicName;
        }
        
        public async Task Notify(Supply supply, CancellationToken cancellationToken)
        {
            if (supply == null)
            {
                return;
            }

            var payload = new SupplyShipped
            {
                SupplyId = supply.Id,
                Items = supply.Items.Select(x => new SupplyShipped.Item(x.Sku, x.Quantity)).ToList()
            };

            await _producerWithKey.ProduceAsync(
                _topicName,
                new Message<string, string>
                {
                    Key = payload.SupplyId.ToString(), 
                    Value = JsonConvert.SerializeObject(payload, Settings)
                },
                cancellationToken);
        }
    }
}