using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svendeproeve_backend.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public virtual ICollection<ProductBrandCollection> ProductBrandCollections { get; set; }
    }
}