using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingDB.Dtos;
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
        List<CustomerGetDto> GetCustomersInIntervalAsync(DateTime from, DateTime untill);
        Task<UserGetDto> GetUserByIdAsync(int userId);

        Task<CustomerGetDto> GetCustomerInfoAsync(int userId);
    }
}
