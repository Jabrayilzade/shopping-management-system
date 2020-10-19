using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingDB.Models;

namespace ShoppingDB.Dtos
{
    public class ProductsFromBasket
    {
        public ProductsFromBasket()
        {
            Photos = new List<Photo>();
        }
        public string Name { get; set; }
        public bool InStock { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
