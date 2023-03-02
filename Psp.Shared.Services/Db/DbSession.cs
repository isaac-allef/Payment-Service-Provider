using System.Data;
using Npgsql;

namespace Psp.Shared.Services.Db;

public sealed class DbSession : IDisposable
{
    public IDbConnection Connection { get; }
    public IDbTransaction? Transaction { get; set; }

    public DbSession(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        builder.Pooling = true;
        builder.MinPoolSize = 0;
        builder.MaxPoolSize = 20;

        Connection = new NpgsqlConnection(builder.ConnectionString);
        Connection.Open();
    }

    public void Dispose() => Connection?.Dispose();
}
