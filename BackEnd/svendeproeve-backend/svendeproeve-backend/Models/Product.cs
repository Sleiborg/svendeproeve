using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace svendeproeve_backend.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public string title { get; set; }
        public string descriptions { get; set; }
        public int price { get; set; }
        public string image { get; set; }


        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }
        public virtual category Category { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
        
        //public virtual ICollection<ProductBrandCollection> ProductBrandCollections { get; set; }
        
    }
}
