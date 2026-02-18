namespace Clinic.Api.Storage;

public sealed class TableClientFactory : ITableClientFactory
{
    public object GetClient(string tableName)
    {
        // TODO: return Azure.Data.Tables.TableClient by tableName.
        return new object();
    }
}
