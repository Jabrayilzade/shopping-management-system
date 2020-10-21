using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql;
using ShoppingDB.DataContext;
using ShoppingDB.Dtos;
using ShoppingDB.Models;

namespace ShoppingDB.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDataContext context;
        private readonly IMapper mapper;
        private readonly IDbConnectionFactory dbConnection;

        public CustomerService(AppDataContext context, IMapper mapper, IDbConnectionFactory dbConnection)
        {
            this.mapper = mapper;
            this.context = context;
            this.dbConnection = dbConnection;
        }

        public async Task AddProductToBasketAsync(Product product, int currentUserId)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            {
                var customer = await context.Customers.FirstOrDefaultAsync(c => c.UserId == currentUserId);
                var customerBasket = await context.Baskets.FirstOrDefaultAsync(c => c.CustomerId == customer.Id);
                customerBasket.ProductsId.Add(product.Id);
                await connection.ExecuteAsync("update products set number_of_products = number_of_products - 1 where products.id = @product_id", new { product_id = product.Id });
                if (product.NumberOfProducts < 1)
                    await connection.ExecuteAsync("update products set in_stock = false where products.id = @product_id", new { product_id = product.Id });
                await context.SaveChangesAsync();
            }
        }

        public Task<List<ProductsFromBasket>> CompleteShoppingAsync(int currentUserId)
        {
            using (IDbConnection connection = dbConnection.CreateDbConnection())
            {
                var myProducts = GetMyBasketAsync(currentUserId);
                return myProducts;
            }
        }

        public async Task ClearMyBasketAsync(int currentUserId)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            var customer = await context.Customers.FirstOrDefaultAsync(c => c.UserId == currentUserId);
            var customerBasket = await context.Baskets.FirstOrDefaultAsync(c => c.CustomerId == customer.Id);
            //Basket customerBasket = await connection.QueryFirstOrDefaultAsync<Basket>("select baskets.ProductsId from baskets where baskets.customer_id = (select user_id from customers where customers.user_id = @current_user_id)", new { current_user_id = currentUserId });
            if (customerBasket.ProductsId.Count == 0)
                return;

            foreach (var product in customerBasket.ProductsId)
                await connection.ExecuteAsync("update products set number_of_products = number_of_products + 1 where products.id = @product_id", new { product_id = product });

            customerBasket.ProductsId.Clear();
            await context.SaveChangesAsync();
        }

        public async Task<List<ProductsFromBasket>> GetMyBasketAsync(int currentUserId)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            var customerId = await connection.QueryFirstOrDefaultAsync<int>("select id from customers where customers.user_id = @current_user_id", new { current_user_id = currentUserId });
            var customerBasket = await context.Baskets.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            List<ProductsFromBasket> basketProducts = new List<ProductsFromBasket>();
            foreach (var productId in customerBasket.ProductsId)
            {
                var product = await context.Products.FindAsync(productId);
                basketProducts.Add(mapper.Map<Product, ProductsFromBasket>(product));
            }
            return basketProducts;
        }

        public async Task RemoveProductFromBasketAsync(int productId, int currentUserId)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            var customerId = await connection.QueryFirstOrDefaultAsync<int>("select id from customers where customers.user_id = @current_user_id", new { current_user_id = currentUserId });
            var customerBasket = await context.Baskets.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            foreach (var id in customerBasket.ProductsId)
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product.Id == productId)
                    product.NumberOfProducts++;
            }

            for (int index = 0; index < customerBasket.ProductsId.Count; index++)
                if (customerBasket.ProductsId.ElementAt(index) == productId)
                    customerBasket.ProductsId.RemoveAt(index);

            await context.SaveChangesAsync();
        }

        public async Task<Product> SearchProductByNameAsync(string productName)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            Product product = await connection.QueryFirstOrDefaultAsync<Product>("select id, name, in_stock as inStock, number_of_products as numberOfProducts from products where name = @product_name", new { product_name = productName });
            return product;
        }

        public List<ProductSearchDto> ViewAllProductsAsync()
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            return connection.Query<ProductSearchDto>("select name, in_stock as inStock, number_of_products as numberOfProducts from products").ToList();
        }
        public List<ProductSearchDto> SearchProductAsync(string searchString)
        {
            using IDbConnection connection = dbConnection.CreateDbConnection();
            return connection.Query<ProductSearchDto>("select name, photos, in_stock as inStock, number_of_products as numberOfProducts from products where name ilike @str ", new { str = searchString + "%" }).ToList();
        }

    }
}



