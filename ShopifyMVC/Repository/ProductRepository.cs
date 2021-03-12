using ShopifyMVC.Data;
using ShopifyMVC.Models;
using ShopifyMVC.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db): base(db)
        {

        }
        public void Update(Product obj)
        {
            _db.Update(obj);
        }
    }
}
