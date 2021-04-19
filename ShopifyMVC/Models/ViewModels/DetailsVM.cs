using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Models.ViewModels
{
    public class DetailsVM
    {
        public Product Product { get; set; }
        public bool ExistInCart { get; set; }
    }
}
