using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mango.web.SD;

namespace Mango.web.Models
{
    public class APIRequest
    {
        public ApiType ApyType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
