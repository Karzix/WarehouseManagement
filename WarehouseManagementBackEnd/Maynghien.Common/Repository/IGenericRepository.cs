
using MayNghien.Common.Models;
using MayNghien.Common.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Maynghien.Common.Repository
{
    public interface IGenericRepository<T, C> where T : BaseEntity where C : BaseContext
    {
        void ClearTracker();
        DbSet<T> GetSet();
        T? Get(Guid id);
        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        void AddRange(List<T> entities, bool isCommit = true);

        void DeleteRange(List<T> entities);
        void SoftDeleteRange(List<T> entities);
        Task<int> CountRecordsAsync(Expression<Func<T, bool>> predicate);
        void BulkInsert(IList<T> items, int packageSize = 1000);
        Task BulkInsert(IList<T> entities, CancellationToken cancellationToken);
        Task BulkUpdate(IList<T> entities, CancellationToken cancellationToken);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
    }
}
