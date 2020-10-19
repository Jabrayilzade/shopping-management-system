using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingDB.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public Basket Basket { get; set; }
        public User User { get; set; }
    }
}
