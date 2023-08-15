using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Service_Center
    {
        public Service_Center() { }

        #region Model
        private string id;
        private string name;
        private string loc_detail;
        private string tel;
        private string url;
        private float longitude;
        private float latitude;

        public static string GetName { get { return "SERVICE_CENTER"; } }
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

        public string Loc_Detail
        {
            get { return loc_detail; }
            set { loc_detail = value; }
        }

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        public string TIMG_URL
        {
            get { return url; }
            set { url = value; }
        }

        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        #endregion
    }
}
