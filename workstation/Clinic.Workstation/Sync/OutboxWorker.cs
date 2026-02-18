namespace Clinic.Workstation.Sync;

public sealed class OutboxWorker
{
    public Task RunOnceAsync(CancellationToken ct)
    {
        // TODO: Read pending outbox rows, dispatch API calls with idempotency keys,
        // update local entity states, and write conflict records on 409/412.
        return Task.CompletedTask;
    }
}
