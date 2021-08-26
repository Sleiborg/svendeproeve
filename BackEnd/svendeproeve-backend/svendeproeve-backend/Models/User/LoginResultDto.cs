using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models.User
{
    public class LoginResultDto
    {
        public string AccessToken { get; set; }

        public DateTime Expires { get; set; }

        public string RefreshToken { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
