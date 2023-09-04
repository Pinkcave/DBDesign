using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Recycle_Order
    {
        public Recycle_Order() { Images = new List<string>(); }
        private static string TABLE_NAME = "RECYCLE_ORDER";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string orderid;
        private Device device;
        private string userid;
        private float expectedprice;
        private string location;
        private string customer_location;
        private string purchase_channel;
        private string storage_capacity;
        private DateTime time;

        public string OrderID
        {
            get { return orderid; }
            set { orderid = value; }
        }

        public Device Device
        {
            get { return device; }
            set { device = value; }
        }

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        public float ExpectedPrice
        {
            get { return expectedprice; }
            set { expectedprice = value; }
        }

        public string Recycle_Location
        {
            get { return location; }
            set { location = value; }
        }
        public DateTime Recycle_Time
        {
            get { return time; }
            set { time = value; }
        }

        public List<string> Images
        {
            get; set;
        }

        public string CustomerLocation
        {
            get { return customer_location; }
            set { customer_location = value; }
        }

        
        #endregion
    }
}
