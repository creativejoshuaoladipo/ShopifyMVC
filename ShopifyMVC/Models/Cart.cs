using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? CartStatusId { get; set; }
        public Product Product { get; set; }
        public CartStatus CartStatus { get; set; }

    }
}
