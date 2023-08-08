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
        private float repairprice;

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

        public string RepairCategoryID
        {
            get { return repaircategoryid.ID; }
            set { repaircategoryid.ID = value; }
        }

        public string CateId
        {
            get { return repaircategoryid.ID; }
            set { repaircategoryid.ID = value; }
        }

        public string CateName
        {
            get { return repaircategoryid.Name; }
            set { repaircategoryid.Name = value; }
        }

        public string CateImage
        {
            get { return repaircategoryid.Image; }
            set { repaircategoryid.Image = value; }
        }

        public string CateDetail
        {
            get { return repaircategoryid.Detail; }
            set { repaircategoryid.Detail = value; }
        }

        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public float RepairPrice
        {
            get { return repairprice; }
            set { repairprice = value; }
        }
        #endregion
    }
}
