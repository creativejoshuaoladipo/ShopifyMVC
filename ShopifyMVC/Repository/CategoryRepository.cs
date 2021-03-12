using ShopifyMVC.Data;
using ShopifyMVC.Models;
using ShopifyMVC.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
       private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db): base(db)
        {

        }
        public void Update(Category obj)
        {
            _db.Update(obj);
        }
    }
}
