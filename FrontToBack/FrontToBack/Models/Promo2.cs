using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Promo2 : BaseEntity
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public bool IsSmall { get; set; }
        public string ImageUrl { get; set; }
        public string RedirectedUrl { get; set; }
        public string ButtonUrl { get; set; }
        public string ButtonText { get; set; }

    }
}
