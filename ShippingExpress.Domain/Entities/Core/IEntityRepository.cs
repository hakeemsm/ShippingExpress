using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using ShippingExpress.Domain.Entities.Extensions;

namespace ShippingExpress.Domain.Entities.Core
{
    public interface IEntityRepository<T> where T : class,IEntity, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All { get; }
        IQueryable<T> GetAll();
        T GetSingle(Guid key);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        PaginatedList<T> Paginate<TKey>(int pageIdx, int pageSize, Expression<Func<T, TKey>> keySelector);
        PaginatedList<T> Paginate<TKey>(int pageIdx, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();


    }

    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntity, new()
    {
        private readonly DbContext _dbContext;

        public EntityRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            _dbContext = dbContext;
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return includeProperties.Aggregate(query, (current, expression) => current.Include(expression));
        }

        public IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public T GetSingle(Guid key)
        {
            return GetAll().FirstOrDefault(k => k.Key == key);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public PaginatedList<T> Paginate<TKey>(int pageIdx, int pageSize, Expression<Func<T, TKey>> keySelector)
        {
            return Paginate(pageIdx, pageSize, keySelector, null);
        }

        public PaginatedList<T> Paginate<TKey>(int pageIdx, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AllIncluding(includeProperties).OrderBy(keySelector);
            query = predicate == null ? query : query.Where(predicate);
            return query.ToPaginatedList(pageIdx, pageSize);
        }

        public void Add(T entity)
        {
            DbEntityEntry<T> dbEntityEntry = _dbContext.Entry<T>(entity);
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
