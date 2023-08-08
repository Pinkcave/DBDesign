using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Service_Loc
    {
        public Service_Loc() { }
        #region Model
        private string id;
        private string loc_name;
        private string loc_detail;
        private string customerid;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public static string GetName { get { return "SERVICE_LOC"; } }

        public string Location_Name
        {
            get { return loc_name; }
            set { loc_name = value; }
        }

        public string Loc_Detail
        {
            get { return loc_detail; }
            set { loc_detail = value; }
        }

        public string CustomerID
        {
            get { return customerid; }
            set { customerid = value; }
        }
        #endregion
    }
}
