using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public bool Success { get; set; }
        public string Reson { get; set; }
    }
}
