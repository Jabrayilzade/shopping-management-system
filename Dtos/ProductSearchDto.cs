using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingDB.Models;

namespace ShoppingDB.Dtos
{
    public class ProductSearchDto
    {
        public string Name { get; set; }
        public bool InStock { get; set; }
        public int NumberOfProducts { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
