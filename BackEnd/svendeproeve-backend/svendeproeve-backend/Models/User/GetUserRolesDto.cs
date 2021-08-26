using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models.User
{
    public class GetUserRolesDto
    {
        public string Name { get; set; }

        public bool HasRole { get; set; }
    }
}
