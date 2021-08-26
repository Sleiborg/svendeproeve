using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models.User
{
    public class RefreshTokenResultDto
    {
        [Key]
        public string Id { get; set; }


        public string Accesstoken { get; set; }

        public DateTime Expires { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
