using AutoMapper;
using ShopifyMVC.Models;
using ShopifyMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.ShopifyMapper
{
    public class ShopifyMapping : Profile
    {
        public ShopifyMapping()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();
        }
    }
}
