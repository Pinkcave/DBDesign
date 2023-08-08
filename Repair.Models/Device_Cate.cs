using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Device_Cate
    {
        public Device_Cate() { }
        private static string TABLE_NAME = "DEVICE_CATE";
        public static string GetName { get { return TABLE_NAME; } }

        #region Model
        private string categoryid;
        private string categoryname;

        public string CategoryID
        {
            get { return categoryid; }
            set { categoryid = value; }
        }

        public string CategoryName
        {
            get { return categoryname; }
            set { categoryname = value; }
        }
        #endregion
    }
}
