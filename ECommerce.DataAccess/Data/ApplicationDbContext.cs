using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.Models.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Hosting;
using Azure;

namespace ECommerce.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ProductCategoryModel> ProductCategories { get; set; }
        public DbSet<ShoppingCartModel> Shoppingcarts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductModel>()
                    .HasMany(e => e.Categories)
                    .WithMany(e => e.Products)
                    .UsingEntity<ProductCategoryModel>(
                         l => l.HasOne<CategoryModel>(e => e.Category).WithMany(e => e.ProductCategories),
                         r => r.HasOne<ProductModel>(e => e.Product).WithMany(e => e.ProductCategories));

            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel { Id = 1, Name = "Electronics", DisplayOrder = 1, CreateDate = DateTime.Now, IsActive = true },
                new CategoryModel { Id = 2, Name = "Fashion", DisplayOrder = 2, CreateDate = DateTime.Now, IsActive = true },
                new CategoryModel { Id = 3, Name = "Furniture", DisplayOrder = 3, CreateDate = DateTime.Now, IsActive = true },
                new CategoryModel { Id = 4, Name = "Food", DisplayOrder = 4, CreateDate = DateTime.Now, IsActive = true },
                new CategoryModel { Id = 5, Name = "Toys and Hobbies", DisplayOrder = 5, CreateDate = DateTime.Now, IsActive = true }
            );

            modelBuilder.Entity<ProductModel>().HasData(
                new ProductModel
                {
                    Id = 1,
                    Name = "WaterProof Bluetooth Smart Watch",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    Code = "SWD9999001",
                    Price = 90,
                    DiscountedPrice = 80,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                },
                new ProductModel
                {
                    Id = 2,
                    Name = "Best Seller Drifit Shorts Unisex for Men",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    Code = "CAW777777701",
                    Price = 30,
                    DiscountedPrice = 20,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                },
                new ProductModel
                {
                    Id = 3,
                    Name = "Foldable Shoe Rack",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    Code = "RITO5555501",
                    Price = 50,
                    DiscountedPrice = 40,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                },
                new ProductModel
                {
                    Id = 4,
                    Name = "Spicy Noodles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    Code = "WS3333333301",
                    Price = 65,
                    DiscountedPrice = 50,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                },
                new ProductModel
                {
                    Id = 5,
                    Name = "Water Gun",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    Code = "SOTJ1111111101",
                    Price = 27,
                    DiscountedPrice = 20,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                },
                new ProductModel
                {
                    Id = 6,
                    Name = "Wireless Headphones",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    Code = "FOT000000001",
                    Price = 23,
                    DiscountedPrice = 20,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                }
                );
        }
    }
}
