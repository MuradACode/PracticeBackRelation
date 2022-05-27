using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Slider : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Desc { get; set; }
        public string RedirectedUrl { get; set; }
        public string RedirectedUrlText { get; set; }
    }
}
