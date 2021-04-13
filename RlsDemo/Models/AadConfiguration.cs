using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RlsDemo.Models
{
    public class AadConfiguration
    {
        public string AuthorityUri { get; set; }
        public string ClientId { get; set; }
        public string[] Scope { get; set; }
        public string ClientSecret { get; set; }
    }
}
