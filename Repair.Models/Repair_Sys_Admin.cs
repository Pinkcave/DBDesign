using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Repair_Sys_Admin
    {
        public Repair_Sys_Admin() { }
        #region Model
        private string id;
        private string pwd;
        private string name;

        private static string TABLE_NAME = "REPAIR_SYS_ADMIN";

        public static string GetName { get { return TABLE_NAME; } }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion
    }
}
