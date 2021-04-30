using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        [Range(0,100, ErrorMessage ="You can takemore than 100 item at a time")]
        public int numberOfItemSelected { get; set; }

        public Product()
        {
            Cart = new HashSet<Cart>();
        }
    }
}
