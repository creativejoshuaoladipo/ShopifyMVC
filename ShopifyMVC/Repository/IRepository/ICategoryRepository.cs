using ShopifyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Repository.IRepository
{
   public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}
