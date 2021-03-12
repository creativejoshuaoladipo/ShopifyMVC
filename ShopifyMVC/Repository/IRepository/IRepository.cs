using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopifyMVC.Repository.IRepository
{
    public interface IRepository<TbEntity> where TbEntity : class
    {
        IEnumerable<TbEntity> GetAll(Expression<Func<TbEntity,bool>> flter = null,
                                     Func<IQueryable<TbEntity>, IOrderedQueryable<TbEntity>> orderBy = null,
                                     string includeProperties = null,
                                     bool isTracking = true);
        TbEntity FirstOrDefault(Expression<Func<TbEntity, bool>> filter = null,
                                string includeProperties = null,
                                bool isTracking = true);
        void Add(TbEntity entity);
        TbEntity Find(int id);
        void Remove(TbEntity entity);
        void RemoveRange(IEnumerable<TbEntity> entities);
        void Save();
    }
}
