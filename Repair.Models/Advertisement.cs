using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Advertisement
    {
        public Advertisement() { }
        private static string TABLE_NAME = "ADVERTISE";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string id;
        private string image;
        private int status;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        #endregion
    }
}
