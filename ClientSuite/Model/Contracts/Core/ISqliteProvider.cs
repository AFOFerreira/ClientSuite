using SQLite;

namespace ClientSuite.Model.Contracts.Core
{

    public interface ISqliteProvider
    {
        SQLiteAsyncConnection Connection { get; }
        Task EnsureCreatedAsync();
        Task RunInTransactionAsync(Action<SQLiteConnection> action);
    }
}
