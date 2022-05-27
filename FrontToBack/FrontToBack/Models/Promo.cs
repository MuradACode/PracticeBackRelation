using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Promo : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string RedirectedUrl { get; set; }
    }
}
