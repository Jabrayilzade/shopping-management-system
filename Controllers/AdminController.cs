using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingDB.DataContext;
using ShoppingDB.Dtos;
using ShoppingDB.Models;
using ShoppingDB.Services;
using ShoppingDB.Helpers;

namespace ShoppingDB.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly AppDataContext context;
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        public AdminController(AppDataContext context, IAdminService adminService, IMapper mapper, IHttpContextAccessor httpContextAccessorm, IAuthService authService)
        {
            this.context = context;
            this.adminService = adminService;
            this.mapper = mapper;
            this.authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var customers = await adminService.GetCustomersAsync();
            return Ok(customers);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersInIntervalAsync(int year, int month, int day)
        {
            DateTime interval = new DateTime(year, month, day);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await adminService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductByIdAsync(int productId)
        {
            var product = await adminService.GetProductByIdAsync(productId);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsInStockAsync()
        {
            var products = await adminService.GetProductsInStockAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProductAsync([FromBody] ProductAddDto newProduct)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Name == newProduct.Name);
            if (product == null)
            {
                product = mapper.Map<Product>(newProduct);
                product.UploadDate = DateTime.Now;
                if (product.NumberOfProducts == 0)
                    product.InStock = false;
                else
                    product.InStock = true;
                await adminService.AddProductAsync(product);
                await adminService.SaveAllAsync();
                return Ok();
            }
            product.NumberOfProducts = product.NumberOfProducts + newProduct.NumberOfProducts;
            await adminService.SaveAllAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            await adminService.DeleteProductAsync(productId);
            await adminService.SaveAllAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(int productId, ProductUpdateDto updatedProduct)
        {
            var product = await context.Products.FindAsync(productId);
            product.Name = updatedProduct.Name;
            product.NumberOfProducts = updatedProduct.NumberOfProducts;
            if (product.NumberOfProducts == 0)
                product.InStock = false;
            else
                product.InStock = true;
            product.EditDate = DateTime.Now;
            await adminService.SaveAllAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("AdminRegister")]
        public async Task<IActionResult> AdminRegisterAsync([FromBody] UserRegisterDto regUser)
        {
            if (await authService.UserExistsAsync(regUser.UserName))
                ModelState.AddModelError("UserName", "Username already exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addUser = new User
            {
                UserName = regUser.UserName,
                UserRole = "admin",
                RoleId = 1,
                RegisterDate = DateTime.Now,
                LastLog = DateTime.Now
            };

            var newUser = await authService.RegisterAsync(addUser, regUser.Password);
            return StatusCode(201, newUser);
        }
    }
}
