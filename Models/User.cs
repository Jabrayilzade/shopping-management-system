using System;
using System.Collections.Generic;
namespace ShoppingDB.Models
{
    public class User
    {
        public User()
        {
            Customers = new List<Customer>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastLog { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public Role Role { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
