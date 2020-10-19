using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingDB.Dtos;
using ShoppingDB.Models;

namespace ShoppingDB.Services
{
    public interface ICustomerService
    {
        Task AddProductToBasketAsync(Product product, int currentUserId);
        Task RemoveProductFromBasketAsync(int productId, int currentUserId);
        Task<List<ProductsFromBasket>> CompleteShoppingAsync(int currentUserId);
        Task<List<ProductsFromBasket>> GetMyBasketAsync(int currentUserId);
        Task ClearMyBasketAsync(int currentUserId);
        Task<Product> SearchProductByNameAsync(string productName);
        List<ProductSearchDto> ViewAllProductsAsync();
        List<ProductSearchDto> SearchProductAsync(string searchString);
    }
}
