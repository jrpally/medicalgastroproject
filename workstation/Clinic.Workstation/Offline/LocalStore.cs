using Microsoft.Data.Sqlite;

namespace Clinic.Workstation.Offline;

public sealed class LocalStore(string connectionString)
{
    public async Task InitializeAsync(CancellationToken ct)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(ct);
        var command = connection.CreateCommand();
        command.CommandText = @"
CREATE TABLE IF NOT EXISTS outbox (
  id TEXT PRIMARY KEY,
  aggregate_type TEXT NOT NULL,
  aggregate_id TEXT NOT NULL,
  operation TEXT NOT NULL,
  payload_json TEXT NOT NULL,
  idempotency_key TEXT NOT NULL UNIQUE,
  attempt_count INTEGER NOT NULL DEFAULT 0,
  status TEXT NOT NULL,
  created_utc TEXT NOT NULL
);";
        await command.ExecuteNonQueryAsync(ct);
    }
}
