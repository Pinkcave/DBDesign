using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{

    public class Device_Evaluation
    {
        public Device_Evaluation() { }
        private static string TABLE_NAME = "DEVICE_EVALUATION";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string deviceid;
        private string evaluation;
        private float price;

        public string DeviceID
        {
            get { return deviceid; }
            set { deviceid = value; }
        }

        public string Evaluation
        {
            get { return evaluation; }
            set { evaluation = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }
        #endregion
    }
}
