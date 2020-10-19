using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingDB.Models;

namespace ShoppingDB.Services
{
    public interface IAdminService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductsInStockAsync();

        Task AddProductAsync(Product newProduct);
        Task DeleteProductAsync(int productId);
        Task<bool> SaveAllAsync();
        Task<List<Customer>> GetCustomersAsync();
        Task<List<Customer>> GetCustomersInIntervalAsync();
    }
}
