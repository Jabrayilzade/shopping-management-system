using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingDB.Dtos;
using ShoppingDB.Models;

namespace ShoppingDB.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductsFromBasket>();
            CreateMap<ProductAddDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductsFromBasket>();
            CreateMap<Product, ProductSearchDto>();

            CreateMap<User, Customer>();
        }
    }
}
