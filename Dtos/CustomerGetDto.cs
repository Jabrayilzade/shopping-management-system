using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingDB.Dtos
{
    public class CustomerGetDto
    {
        public string UserName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastLog { get; set; }
        public int BasketId { get; set; }
    }
}
