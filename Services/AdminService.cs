using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingDB.DataContext;
using ShoppingDB.Models;

namespace ShoppingDB.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDataContext context;
        public AdminService(AppDataContext context)
        {
            this.context = context;
        }

        public async Task AddProductAsync(Product newProduct)
        {
            await context.Products.AddAsync(newProduct);
        }

        public async Task DeleteProductAsync(int productId)
        {
            context.Products.Remove(new Product
            {
                Id = productId
            });
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await context.Products.Include(c => c.Photos).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await context.Products.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<Product>> GetProductsInStockAsync()
        {
            return await context.Products.Where(p => p.InStock == true).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await context.Customers.ToListAsync();
        }

        public Task<List<Customer>> GetCustomersInIntervalAsync()
        {
            throw new NotImplementedException();
        }
    }
}
