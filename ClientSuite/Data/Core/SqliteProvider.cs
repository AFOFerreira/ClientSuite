using ClientSuite.Model.Contracts.Core;
using ClientSuite.Model.Entities;
using SQLite;
public sealed class SqliteProvider : ISqliteProvider
{
    private readonly Lazy<SQLiteAsyncConnection> _lazyConn;

    public SqliteProvider()
    {
        var dbPath = Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                   "clientsuite.db3");

        var flags = SQLiteOpenFlags.ReadWrite
                   | SQLiteOpenFlags.Create
                   | SQLiteOpenFlags.FullMutex; // bom no Windows

        _lazyConn = new(() => new SQLiteAsyncConnection(
            dbPath,
            flags,
            storeDateTimeAsTicks: false
        ));
    }

    public SQLiteAsyncConnection Connection => _lazyConn.Value;

    public async Task EnsureCreatedAsync()
    {
        await Connection.CreateTableAsync<Client>();
        await Connection.CreateTableAsync<ClientAddress>();

    }

    public Task RunInTransactionAsync(Action<SQLiteConnection> action)
        => Connection.RunInTransactionAsync(action);
}
