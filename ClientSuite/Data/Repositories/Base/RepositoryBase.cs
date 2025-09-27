using ClientSuite.Model.Contracts.Core;
using ClientSuite.Model.Contracts.Repositories.Base;
using ClientSuite.Model.Entities.Base;
using SQLite;
using System.Linq.Expressions;

namespace ClientSuite.Data.Repositories.Base
{
    public class RepositoryBase<T>(ISqliteProvider provider) : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly SQLiteAsyncConnection _conn = provider.Connection;

        public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            _conn.Table<T?>()
                 .Where(x => x.Id == id)
                 .FirstOrDefaultAsync();

        public Task<List<T>> ListAsync(CancellationToken ct = default) =>
            _conn.Table<T>().ToListAsync();

        public Task<int> InsertAsync(T entity, CancellationToken ct = default) =>
            _conn.InsertAsync(entity);

        public Task<int> UpdateAsync(T entity, CancellationToken ct = default) =>
            _conn.UpdateAsync(entity);

        public Task<int> DeleteAsync(T entity, CancellationToken ct = default) =>
            _conn.DeleteAsync(entity);

        public Task<T> FirstAsync(CancellationToken ct = default) =>
            _conn.Table<T>().FirstAsync();

        public Task<T?> FirstOrDefaultAsync(CancellationToken ct = default) =>
            _conn.Table<T?>().FirstOrDefaultAsync();

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
            _conn.Table<T>().Where(predicate).FirstAsync();

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
            _conn.Table<T?>().Where(predicate).FirstOrDefaultAsync();

        public Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
            _conn.Table<T>().Where(predicate).ToListAsync();

        public Task<List<T>> ListAsync<TOrder>(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TOrder>>? orderBy = null,
            bool descending = false,
            int? skip = null,
            int? take = null,
            CancellationToken ct = default)
        {
            var q = _conn.Table<T>();
            if (predicate != null) q = q.Where(predicate);
            if (orderBy != null) q = descending ? q.OrderByDescending(orderBy) : q.OrderBy(orderBy);
            if (skip.HasValue) q = q.Skip(skip.Value);
            if (take.HasValue) q = q.Take(take.Value);
            return q.ToListAsync();
        }

        public async Task<(IReadOnlyList<T> Items, int Total)> GetPageAsync<TOrder>(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, TOrder>>? orderBy = null,
            bool descending = false,
            int page = 1,
            int pageSize = 20,
            CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            var qBase = _conn.Table<T>();
            if (predicate != null) qBase = qBase.Where(predicate);

            var total = await qBase.CountAsync();

            var q = qBase;
            if (orderBy != null) q = descending ? q.OrderByDescending(orderBy) : q.OrderBy(orderBy);
            q = q.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await q.ToListAsync();
            return (items, total);
        }
    }
}
