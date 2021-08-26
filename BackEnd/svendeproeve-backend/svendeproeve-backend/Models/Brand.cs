using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace svendeproeve_backend.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        //public virtual ICollection<ProductBrandCollection> ProductBrandCollections { get; set; }
    }
}