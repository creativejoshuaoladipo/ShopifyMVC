using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public bool? IsFeatured { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public ICollection<Cart> Cart { get; set; }
        public Category Category { get; set; }

        public Product()
        {
            Cart = new HashSet<Cart>();
        }
    }
}
