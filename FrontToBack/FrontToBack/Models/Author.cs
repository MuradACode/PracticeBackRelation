using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Author : BaseEntity
    {
        public string Fullname { get; set; }
        public List<Product> Products { get; set; }
    }
}
