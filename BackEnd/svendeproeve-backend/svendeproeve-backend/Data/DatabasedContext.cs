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

        public DbSet<Product> themes { get; set; }

        public DbSet<AppRefreshToken> AppRefreshTokens { get; set; }
    }
}
