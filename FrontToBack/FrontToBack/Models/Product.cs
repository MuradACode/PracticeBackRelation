using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public int Discount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
