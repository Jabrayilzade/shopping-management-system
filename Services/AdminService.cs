using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using NpgsqlTypes;
using ShoppingDB.DataContext;
using ShoppingDB.Dtos;
using ShoppingDB.Models;

namespace ShoppingDB.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDataContext context;
        private readonly IDbConnectionFactory dbConnection;
        public AdminService(AppDataContext context, IDbConnectionFactory dbConnection)
        {
            this.dbConnection = dbConnection;
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

        public List<CustomerGetDto> GetCustomersInIntervalAsync(DateTime from, DateTime untill)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            var query = "select * from customer_info as cv where cv.lastLog <= @untill_date and cv.lastLog >= @from_date";
            var loggedCustomers = connection.Query<CustomerGetDto>(query, new { untill_date = untill, from_date = from}).ToList();
            return loggedCustomers;
        }

        public async Task<UserGetDto> GetUserByIdAsync(int userId)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            var queryResult = await connection.QueryFirstAsync<String>("get_user_info", new { user_id = userId }, commandType: CommandType.StoredProcedure);
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            UserGetDto userGetDto = JsonConvert.DeserializeObject<UserGetDto>(queryResult, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return userGetDto;
        }

        public async Task<CustomerGetDto> GetCustomerInfoAsync(int customerId)
        {
            var query = "select * from customer_info as cv where cv.customerId = @customer_id";
            using IDbConnection connection = dbConnection.CreateDbConnection();
            var customer = await connection.QueryFirstAsync<CustomerGetDto>(query, new { customer_id = customerId });
            return customer;
        }
    }
}
