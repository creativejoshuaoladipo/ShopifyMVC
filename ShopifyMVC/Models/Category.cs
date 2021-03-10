using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new HashSet<Product>();
        }

    }
}
