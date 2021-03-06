using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using svendeproeve_backend.Models;
using svendeproeve_backend.Models.User;

namespace svendeproeve_backend.Data
{
    public class Databasedcontext : IdentityDbContext<AppUser, AppRole, String>
    {
        public Databasedcontext(DbContextOptions<Databasedcontext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<category> Categories { get; set; }

        public DbSet<Brand> Brands { get; set; }
        
        public DbSet<ProductBrandCollection> ProductBrandCollections { get; set; }

        public DbSet<AppRefreshToken> AppRefreshTokens { get; set; }
    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Brand>().HasMany(i => i.Products);
            //builder.Entity<Product>().HasMany(i => i.Brands);

            builder.Entity<ProductBrandCollection>().HasOne(i => i.Product).WithMany(i => i.ProductBrandCollections).HasForeignKey(i => i.productId);
            builder.Entity<ProductBrandCollection>().HasOne(i => i.Brand).WithMany(i => i.ProductBrandCollections).HasForeignKey(i => i.BrandId);
            builder.Entity<ProductBrandCollection>().HasKey(i => new { i.productId, i.BrandId });

            base.OnModelCreating(builder);
        }
    }
}
