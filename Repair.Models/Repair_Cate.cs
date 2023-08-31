using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Repair_Cate
    {
        public Repair_Cate() { }
        private static string TABLE_NAME = "REPAIR_CATE";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string id;
        private string name;
        private string image;
        private string detail;
        private string? description;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        public string? Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion
    }
}
