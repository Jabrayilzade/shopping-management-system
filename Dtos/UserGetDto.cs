using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingDB.Dtos
{
    public class UserGetDto
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public DateTime LastLog { get; set; }
    }
}
