using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Device_Type
    {
        public Device_Type() { }
        private static string TABLE_NAME = "DEVICE_TYPE";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string typeid;
        private string type_name;
        private List<string> url;
        private string brand;
        private string normalprice;

        public string TypeID
        {
            get { return typeid; }
            set { typeid = value; }
        }

        public string Type_Name
        {
            get { return type_name; }
            set { type_name = value; }
        }

        public List<string> Structure_Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        

        #endregion
    }
}
