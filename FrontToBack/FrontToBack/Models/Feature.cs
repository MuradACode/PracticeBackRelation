using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Feature : BaseEntity
    {
        public string IconUrl { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

    }
}
