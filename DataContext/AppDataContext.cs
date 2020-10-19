using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingDB.Models;

namespace ShoppingDB.DataContext
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.ToTable("users");
                x.Property(s => s.Id).HasColumnName("id").IsRequired();
                x.Property(s => s.UserName).HasColumnName("user_name").IsRequired();
                x.Property(s => s.UserRole).HasColumnName("user_role").IsRequired();
                x.Property(s => s.PasswordHash).HasColumnName("password_hash").IsRequired();
                x.Property(s => s.PasswordSalt).HasColumnName("password_salt").IsRequired();
                x.Property(s => s.RegisterDate).HasColumnName("register_date").IsRequired();
                x.Property(s => s.LastLog).HasColumnName("last_log").IsRequired();
                x.Property(s => s.RoleId).HasColumnName("role_id").IsRequired();
            });

            modelBuilder.Entity<Product>(x =>
            {
                x.ToTable("products");
                x.Property(s => s.Id).HasColumnName("id").IsRequired();
                x.Property(s => s.Name).HasColumnName("name").IsRequired();
                x.Property(s => s.UploadDate).HasColumnName("upload_date").IsRequired();
                x.Property(s => s.EditDate).HasColumnName("edit_date").IsRequired();
                x.Property(s => s.InStock).HasColumnName("in_stock").IsRequired();
                x.Property(s => s.NumberOfProducts).HasColumnName("number_of_products").IsRequired();
            });

            modelBuilder.Entity<Photo>(x =>
            {
                x.ToTable("photos");
                x.Property(s => s.Id).HasColumnName("id").IsRequired();
                x.Property(s => s.PhotoUrl).HasColumnName("photo_url").IsRequired();
                x.Property(s => s.UploadDate).HasColumnName("upload_date").IsRequired();
                x.Property(s => s.ProductId).HasColumnName("product_id").IsRequired();
            });

            modelBuilder.Entity<Customer>(x =>
            {
                x.ToTable("customers");
                x.Property(s => s.Id).HasColumnName("id").IsRequired();
                x.Property(s => s.UserName).HasColumnName("user_name").IsRequired();
                x.Property(s => s.UserId).HasColumnName("user_id").IsRequired();
            });

            modelBuilder.Entity<Basket>(x =>
            {
                x.ToTable("baskets");
                x.Property(s => s.Id).HasColumnName("id").IsRequired();
                x.Property(s => s.CustomerId).HasColumnName("customer_id").IsRequired();
                //x.Property(s => s.ProductsId).HasColumnName("products_id").IsRequired();
            });

            modelBuilder.Entity<Role>(x =>
            {
                x.ToTable("roles");
                x.Property(s => s.Id).HasColumnName("id").IsRequired();
                x.Property(s => s.Name).HasColumnName("name").IsRequired();
            });

            Role superAdminRole = new Role { Id = 1, Name = "superadmin" };
            Role adminRole = new Role { Id = 2, Name = "admin" };
            Role managerRole = new Role { Id = 3, Name = "manager" };
            Role advisorRole = new Role { Id = 4, Name = "advisor" };
            Role customerRole = new Role { Id = 5, Name = "user" };
            modelBuilder.Entity<Role>().HasData(new Role[] { superAdminRole, adminRole, managerRole, advisorRole, customerRole });
            base.OnModelCreating(modelBuilder);
        }
    }
}
