using Confluent.Kafka;
using System.Text.Json;

namespace PetCareWebAPI.Services
{
    public interface IKafkaProducer
    {
        Task PublishAsync(string topic, object payload, string? key = null, CancellationToken ct = default);
    }

    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(IConfiguration cfg)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = cfg["Kafka:BootstrapServers"],
                Acks = Acks.All,
                EnableIdempotence = true,
                MessageTimeoutMs = 5000
            };
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public async Task PublishAsync(string topic, object payload, string? key = null, CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(payload);
            var msg = new Message<string, string> { Key = key, Value = value };
            await _producer.ProduceAsync(topic, msg, ct);
        }

        public void Dispose() => _producer?.Dispose();
    }
}
