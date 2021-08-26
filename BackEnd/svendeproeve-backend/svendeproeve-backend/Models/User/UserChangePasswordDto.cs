using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models.User
{
    public class UserChangePasswordDto
    {
        [Required]
        public string Password { get; set; }
    }
}
