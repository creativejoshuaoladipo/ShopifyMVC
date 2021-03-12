using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopifyMVC.Data;
using ShopifyMVC.Repository.IRepository;

namespace ShopifyMVC.Repository
{
    public class Repository<TbEntity> : IRepository<TbEntity> where TbEntity : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<TbEntity> dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbset = _db.Set<TbEntity>();
        }
        public void Add(TbEntity entity)
        {
            dbset.Add(entity);
        }

        public TbEntity Find(int id)
        {
           var entity = dbset.Find(id);
            return entity;
        }

        public TbEntity FirstOrDefault(Expression<Func<TbEntity, bool>> filter = null,
                                        string includeProperties = null, bool isTracking = true)
        {

            //First Store the Dbset datas inside a seperate variable
            //because of the muliple result of the if Statements
            IQueryable<TbEntity> query = dbset;

            //If there are specific condition to be met- write a filter expression then pass it using the where clause
            if (filter != null)
            {
                query = query.Where(filter);
            }

          
            //For cases when the Classes inside a class is more than 1 (having 2 Includes)
            //We will use split to sperate them then add them manually using a loop
            if (includeProperties != null)
            {
                foreach (var includeproperty in includeProperties.Split(new char[] { ',' },
                                                 StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeproperty);
                }
            }

            if (isTracking == false)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();

        }

        public IEnumerable<TbEntity> GetAll(Expression<Func<TbEntity, bool>> filter = null,
                                      Func<IQueryable<TbEntity>, IOrderedQueryable<TbEntity>> orderBy = null,
                                       string includeProperties = null, bool isTracking = true)
        {
            //First Store the Dbset datas inside a seperate variable
            //because of the muliple result of the if Statements
            IQueryable<TbEntity> query = dbset;
            
            //If there are specific condition to be met- write a filter expression then pass it using the where clause
            if(filter != null)
            {
               query = query.Where(filter);
            }

            if(orderBy != null)
            {
               query = orderBy(query);
            }
            //For cases when the Classes inside a class is more than 1 (having 2 Includes)
            //We will use split to sperate them then add them manually using a loop
            if(includeProperties != null)
            {
                foreach(var includeproperty in includeProperties.Split(new char[] {',' },
                                                 StringSplitOptions.RemoveEmptyEntries))
                {
                   query= query.Include(includeproperty);
                }
            }

            if(isTracking == false)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public void Remove(TbEntity entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TbEntity> entities)
        {
            dbset.RemoveRange(entities);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
