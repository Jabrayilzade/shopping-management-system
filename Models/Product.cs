using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingDB.Models
{
    public class Product
    {
        public Product()
        {
            Photos = new List<Photo>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool InStock { get; set; }
        public int NumberOfProducts { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
