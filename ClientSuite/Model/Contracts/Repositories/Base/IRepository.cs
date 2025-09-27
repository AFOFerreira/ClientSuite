using ClientSuite.Model.Entities.Base;
using System.Linq.Expressions;

namespace ClientSuite.Model.Contracts.Repositories.Base
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<int> InsertAsync(T entity, CancellationToken ct = default);
        Task<int> UpdateAsync(T entity, CancellationToken ct = default);
        Task<int> DeleteAsync(T entity, CancellationToken ct = default);
        Task<List<T>> ListAsync(CancellationToken ct = default);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<List<T>> ListAsync<TOrder>(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TOrder>>? orderBy = null,
            bool descending = false,
            int? skip = null,
            int? take = null,
            CancellationToken ct = default);
        Task<T> FirstAsync(CancellationToken ct = default);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<(IReadOnlyList<T> Items, int Total)> GetPageAsync<TOrder>(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TOrder>>? orderBy = null,
            bool descending = false,
            int page = 1,
            int pageSize = 20,
            CancellationToken ct = default);
    }   
}
