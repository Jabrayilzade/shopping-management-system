using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingDB.DataContext;
using ShoppingDB.Dtos;
using ShoppingDB.Helpers;
using ShoppingDB.Models;
using ShoppingDB.Services;

namespace ShoppingDB.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = Roles.Customer)]
    public class UserController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;
        private readonly AppDataContext context;
        private readonly IHttpContextAccessor httpcontext;

        public UserController(IAuthService authService, ICustomerService customerService, IMapper mapper, AppDataContext context, IHttpContextAccessor httpcontext)
        {
            this.authService = authService;
            this.customerService = customerService;
            this.mapper = mapper;
            this.context = context;
            this.httpcontext = httpcontext;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewAllPRoductsAsync()
        {
            List<ProductSearchDto> products = customerService.ViewAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> ViewMyBasket()
        {
            if (HttpContext.User == null)
                return Unauthorized();

            var userId = Int32.Parse(httpcontext.HttpContext.User.FindFirst("Id").Value);
            List<ProductsFromBasket> products = await customerService.GetMyBasketAsync(userId);
            if (products == null)
                return NotFound("Your basket is empty");
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> CompleteShoppingAsync()
        {
            if (HttpContext.User == null)
                return Unauthorized();

            var userId = Int32.Parse(httpcontext.HttpContext.User.FindFirst("Id").Value);
            var products = await customerService.CompleteShoppingAsync(userId);
            if (products == null)
                return NotFound("Ur basket is empty");
            return Ok(products);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SearchProduct(string searchString)
        {
            var products = customerService.SearchProductAsync(searchString);
            return Ok(products);
        }

        [HttpGet]
        public IActionResult SearchProductByNameAsync(string productName)
        {
            var product = customerService.SearchProductByNameAsync(productName);
            if (product == null)
                return NotFound("Product with given name has not found");
            return Ok(product);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProductFromBasketAsync(string productName)
        {
            var userId = Int32.Parse(httpcontext.HttpContext.User.FindFirst("Id").Value);
            Product product = await customerService.SearchProductByNameAsync(productName);
            await customerService.RemoveProductFromBasketAsync(product.Id, userId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearMyBasketAsync()
        {
            var userId = Int32.Parse(httpcontext.HttpContext.User.FindFirst("Id").Value);
            await customerService.ClearMyBasketAsync(userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToBasketAsync(string productName)
        {
            var userId = Int32.Parse(httpcontext.HttpContext.User.FindFirst("Id").Value);
            Product product = await customerService.SearchProductByNameAsync(productName);

            if (product == null)
                return NotFound("Product with given name has not found");

            if (product.NumberOfProducts < 1)
                return NotFound("Product is not in stock");

            await customerService.AddProductToBasketAsync(product, userId);
            return Ok();
        }

        [HttpPost("UserRegister")]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegisterAsync([FromBody] UserRegisterDto regUser)
        {
            if (await authService.UserExistsAsync(regUser.UserName))
                ModelState.AddModelError("UserName", "Username already exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addUser = new User
            {
                UserName = regUser.UserName,
                UserRole = "customer",
                RoleId = 2,
                RegisterDate = DateTime.Now,
                LastLog = DateTime.Now
            };

            var newUser = await authService.RegisterAsync(addUser, regUser.Password);
            var customer = mapper.Map<Customer>(newUser);
            customer.UserId = newUser.Id;
            customer.UserName = newUser.UserName;
            var basket = new Basket
            {
                CustomerId = customer.Id
            };
            await context.Baskets.AddAsync(basket);
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
            return StatusCode(201, newUser);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLoginAsync([FromBody] UserLoginDto logUser)
        {
            var user = await authService.LoginAsync(logUser.UserName, logUser.Password);

            if (user == null)
                return Unauthorized();

            if (user.UserRole == "admin")
            {
                var adminUser = context.Users.Find(user.Id);
                adminUser.LastLog = DateTime.Now;
            }
            if (user.UserRole == "customer")
            {
                var customerUser = context.Users.Find(user.Id);
                customerUser.LastLog = DateTime.Now;
            }
            await context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
