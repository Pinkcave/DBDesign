using System;
using System.Configuration;
using System.Data;
using System.Web;
using Repair.Models;
using Repair.Helper;
using Oracle.ManagedDataAccess.Client;
using System.Text.Json;

namespace Repair.Server
{
    public class DeviceInfo
    {
        public Device device { get; set; }
        public Device_Evaluation eval { get; set; }
    }

    public class DeviceServer
    {
        public static List<Device> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Device.GetName + " where deviceid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Device.GetName;
            }
            List<Device> list = new List<Device>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device Object = new Device();
                    Object.DeviceID = reader.GetString(0);
                    Device_Cate cate = DeviceCateServer.Query(reader.GetString(1)).FirstOrDefault();
                    Object.Device_Cate_ID = cate;
                    Device_Type type = DeviceTypeServer.Query(reader.GetString(2)).FirstOrDefault();
                    Object.Device_Type_ID = type;
                    Object.PurchaseChannel = reader.GetString(3);
                    Object.StorageCapacity = reader.GetString(4);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<Device> QueryByAttribute(string attribute, string value)
        {
            string sql = "select * from device where {0} = {1}";
            sql = string.Format(sql, attribute, value);
            
            List<Device> list = new List<Device>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device Object = new Device();
                    Object.DeviceID = reader.GetString(0);
                    Device_Cate cate = DeviceCateServer.Query(reader.GetString(1)).FirstOrDefault();
                    Object.Device_Cate_ID = cate;
                    Device_Type type = DeviceTypeServer.Query(reader.GetString(2)).FirstOrDefault();
                    Object.Device_Type_ID = type;
                    Object.PurchaseChannel = reader.GetString(3);
                    Object.StorageCapacity = reader.GetString(4);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<DeviceInfo> Search(string[] keys)
        {
            List<DeviceInfo> results = new List<DeviceInfo>();
            List<Device> devices = Query();
            foreach (string key in keys)
            {
                foreach (Device device in devices)
                {
                    if (device.Device_Cate_ID.CategoryName.ToUpper().Contains(key.ToUpper()) 
                        || device.Device_Type_ID.Type_Name.ToUpper().Contains(key.ToUpper()) 
                        || device.Device_Type_ID.Brand.ToUpper().Contains(key.ToUpper()))
                    {
                        Device_Evaluation eval = DeviceEvalServer.Query(device.DeviceID).FirstOrDefault();
                        DeviceInfo info = new DeviceInfo();
                        info.device =device;
                        info.eval = eval;
                        results.Add(info);
                    }
                }
            }
            results = results.DistinctBy(x => x.device.Device_Type_ID.TypeID).ToList();
            return results;
        }

        public static List<DeviceInfo> SearchAll()
        {
            List<DeviceInfo> results = new List<DeviceInfo>();
            List<Device> devices = Query();
           
            foreach (Device device in devices)
            {
                 Device_Evaluation eval = DeviceEvalServer.Query(device.DeviceID).FirstOrDefault();
                 DeviceInfo info = new DeviceInfo();
                 info.device = device;
                 info.eval = eval;
                 results.Add(info);
                
            }
            
            return results;
        }
        public static int Insert(Device device)
        {
            //Device? device = JsonSerializer.Deserialize<Device>(JsonInfo);
            if (device == null)
                return -1;
            //string sql = "insert into " + Device.GetName + " values(" + "\'" + device.DeviceID + "\'," + "\'" + device.Device_Cate_ID.CategoryID + "\'," + "\'" + device.Device_Type_ID.TypeID + "\')";
            string sql = string.Format("insert into {0} values(\'{1}\',\'{2}\',\'{3}\',\'{4}\',\'{5}\')",Device.GetName,device.DeviceID,device.Device_Cate_ID.CategoryID,device.Device_Type_ID.TypeID,device.PurchaseChannel,device.StorageCapacity);
            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Device.GetName + " where deviceid=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(Device device, string old_id)
        {
            //Device? device = JsonSerializer.Deserialize<Device>(JsonInfo);
            if (device == null)
                return -1;
            string sql = "update " + Device.GetName + " set "
                         + "DeviceID=:new_deviceID, Device_Cate_ID=:new_cateID, Device_Type_ID=:new_typeID,Purchase_Channel=:new_Channel,Storage_Capacity=:new_Storage"
                         + " where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_deviceID", OracleDbType.Varchar2, device.DeviceID, ParameterDirection.Input),
                new OracleParameter(":new_cateID", OracleDbType.Varchar2, device.Device_Cate_ID.CategoryID, ParameterDirection.Input),
                new OracleParameter(":new_typeID", OracleDbType.Varchar2, device.Device_Type_ID.TypeID, ParameterDirection.Input),
                new OracleParameter(":new_Channel", OracleDbType.Varchar2, device.PurchaseChannel, ParameterDirection.Input),
                new OracleParameter(":new_Storage", OracleDbType.Varchar2, device.StorageCapacity, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Count()
        {
            string sql = "select tablecount from counter where tablename = \'DEVICE\'";
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                reader.Read();
                return reader.GetInt32(0);
            }
        }
    }
}
