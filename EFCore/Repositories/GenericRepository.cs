using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFCore.EfStructures.Entities;
namespace DataLayer.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {

        private Testproject_dbContext _db;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(Testproject_dbContext db)
        {
            _db = db;

            _dbSet = db.Set<TEntity>();

        }
      
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? where = null)// هرمدل لیستی که از جنس فلان کلاس باشد
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            return query.ToList();

        }
        public virtual TEntity? GetByID(object id)
        {

            return _dbSet.Find(id);
        }
       
        public virtual void insert(TEntity Row)
        {

            _dbSet.Add(Row);

        }
        public virtual void Update(TEntity Row)
        {

            _dbSet.Attach(Row);//میگه برو این ردیف رو دربست بگیرش بگو من باهاش کاردارم
            _db.Entry(Row).State = EntityState.Modified;// و اینجا میگه این ردیف رو که پیدا کردی به روز رسانی کن
        }
        public virtual void Delete(TEntity Row)
        {
            _dbSet.Attach(Row);//میگه برو این ردیف رو دربست بگیرش بگو من باهاش کاردارم

            _db.Entry(Row).State = EntityState.Deleted;// و اینجا میگه این ردیف رو که پیدا کردی به روز رسانی کن
        }
        public virtual void DeleteById(object ID)
        {
            var row = GetByID(ID);
            if (row != null)
            {
                Delete(row);
            }

        }

    }
}
