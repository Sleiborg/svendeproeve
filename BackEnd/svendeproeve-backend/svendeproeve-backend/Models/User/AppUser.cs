using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models.User
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            AppRefreshTokens = new HashSet<AppRefreshToken>();
        }

        public virtual ICollection<AppRefreshToken> AppRefreshTokens { get; set; }
    }
}
