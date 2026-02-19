using System.Text.Json;

namespace Clinic.Api.Storage;

public sealed class QueuePublisher : IQueuePublisher
{
    public Task PublishAsync(string queueName, object payload, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(payload);
        // TODO: send message to Azure Queue Storage / Service Bus.
        _ = json;
        _ = queueName;
        return Task.CompletedTask;
    }
}
