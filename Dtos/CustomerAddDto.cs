using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingDB.Models;

namespace ShoppingDB.Dtos
{
    public class CustomerAddDto
    {
        public int Id { get; set; }
        public int UserName { get; set; }
        public int UserId { get; set; }
        public User User {get; set; }
    }
}
