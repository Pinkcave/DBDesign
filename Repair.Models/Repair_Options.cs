using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Repair_Options
    {
        public Repair_Options() { repaircategoryid = new Repair_Cate(); }
        private static string TABLE_NAME = "REPAIR_OPTION";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string optionid;
        private string repairrequirement;
        private Repair_Cate repaircategoryid;
        private string brand;
        private string warranty;

        public string OptionID
        {
            get { return optionid; }
            set { optionid = value; }
        }

        public string RepairRequirement
        {
            get { return repairrequirement; }
            set { repairrequirement = value; }
        }
        public Repair_Cate RepairCategory
        {
            get { return repaircategoryid; }
            set { repaircategoryid = value; }
        }
       
        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public string isWarranty
        {
            get { return warranty; }
            set { warranty = value; }
        }
        #endregion
    }
}
