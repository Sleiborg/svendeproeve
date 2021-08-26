using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models.User
{
    public class GetUserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
