using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyImageAPI.Models
{
    public class RootDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Large_url { get; set; }
        public int Source_id { get; set; }
        public string Copyright { get; set; }
        public string Site { get; set; }
    }
}
