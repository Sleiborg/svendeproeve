using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models
{
    public class category
    {
        public int categoryId { get; set; }

        public string categoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
