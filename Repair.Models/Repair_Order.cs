using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Repair_Order
    {
        public Repair_Order() { couponid = new Coupon(); couponid.Id = "<null>"; Images = new List<string>(); }
        private static string TABLE_NAME = "REPAIR_ORDER";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string orderid;
        private float? orderprice;
        private Coupon? couponid;
        private string engineerid;
        private string userid;
        private Repair_Options repairoptionid;
        private DateTime createtime;
        private string repairlocation;
        private DateTime repairtime;
        private string? userrate;
        private int orderstatus;
        private string customer_location;

        public string OrderID
        {
            get { return orderid; }
            set { orderid = value; }
        }

        public float? OrderPrice
        {
            get { return orderprice; }
            set { orderprice = value; }
        }

        public string CouponID
        {
            get { return couponid.Id; }
            set { couponid.Id = value; }
        }

        public Coupon CouObj
        {
            get { return couponid; }
            set { couponid = value; }
        }

        public string EngineerID
        {
            get { return engineerid; }
            set { engineerid = value; }
        }

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        public Repair_Options RepairOptionID
        {
            get { return repairoptionid; }
            set { repairoptionid = value; }
        }

        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }

        public string RepairLocation
        {
            get { return repairlocation; }
            set { repairlocation = value; }
        }

        public DateTime RepairTime
        {
            get { return repairtime; }
            set { repairtime = value; }
        }

        public string? UserRate
        {
            get { return userrate; }
            set { userrate = value; }
        }

        public int OrderStatus
        {
            get { return orderstatus; }
            set { orderstatus = value; }
        }

        public List<string> Images
        {
            get;set;
        }

        public string CustomerLocation
        {
            get { return customer_location; }
            set { customer_location = value; }
        }
        #endregion
    }
}
