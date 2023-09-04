using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Device
    {
        public Device() { }
        private static string TABLE_NAME = "DEVICE";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string deviceid;
        private Device_Cate device_cate_id;
        private Device_Type device_type_id;
        private string purchase_channel;
        private string storage_capacity;
        public string DeviceID
        {
            get { return deviceid; }
            set { deviceid = value; }
        }

        public Device_Cate Device_Cate_ID
        {
            get { return device_cate_id; }
            set { device_cate_id = value; }
        }

        public Device_Type Device_Type_ID
        {
            get { return device_type_id; }
            set { device_type_id = value; }
        }

        public string PurchaseChannel
        {
            get { return purchase_channel; }
            set { purchase_channel = value; }
        }

        public string StorageCapacity
        {
            get { return storage_capacity; }
            set { storage_capacity = value; }
        }
        #endregion
    }
}
