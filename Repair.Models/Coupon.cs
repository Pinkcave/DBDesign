using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Coupon
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public float Discount { get; set; }
        public bool Status { get; set; }

        private static string TABLE_NAME = "COUPON";
        public static string GetName { get { return TABLE_NAME; } }
    }
}
