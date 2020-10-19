using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingDB.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
