using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models
{
    public class Product
    {
        public int productId { get; set; }
        public string title { get; set; }
        public string descriptions { get; set; }
        public int price { get; set; }
        public string image { get; set; }


        public int CategoryID { get; set; }
        public virtual category Category { get; set; }

        public virtual ICollection<ProductBrandCollection> ProductBrandCollections { get; set; }
    }
}
