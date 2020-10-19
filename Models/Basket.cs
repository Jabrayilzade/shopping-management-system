using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingDB.Models
{
    public class Basket
    {
        public Basket()
        {
            ProductsId = new List<int>();
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<int> ProductsId { get; set; }
    }
}
