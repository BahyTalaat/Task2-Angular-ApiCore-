using BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BL.Bases
{
    public class BaseRepositroy<T> : IRepository<T> where T : class
    {
        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }
        public BaseRepositroy(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }
       

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(int entityId)
        {
            return DbSet.Find(entityId);
        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return DbSet.FirstOrDefault(filter);
            }
            return null;
        }
        public bool GetAny(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = DbSet;
            bool result = false;
            if (filter != null)
            {
                result = query.Any(filter);
            }
            return result;
        }
        public IQueryable<T> GetAllSorted<TKey>(Expression<Func<T, TKey>> sortingExpression)
        {
            return DbSet.OrderBy<T, TKey>(sortingExpression);
        }
    
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query;
        }
        
        public bool Insert(T entity)
        {
            bool returnVal = false;
            EntityEntry<T> dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
                returnVal = true;
            }
            
            return returnVal;
        }

        public void InsertList(List<T> entityList)
        {

            DbSet.AddRange(entityList);
        }

        public void Update(T entity)
        {
           
            EntityEntry<T> dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void UpdateList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                Update(item);
            }
        }

        public void Delete(T entity)
        {
            
            EntityEntry<T> dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }
        public void Delete(int entityId)
        {
            var entity = GetById(entityId);
            if (entity == null) return;
            Delete(entity);
        }
    }
}
