using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Dtos
{
    public class CreateProjectDto
    {
        public int productId { get; set; }
        public string title { get; set; }
        public string descriptions { get; set; }
        public int price { get; set; }
        public string Base64image { get; set; }
    }
}
