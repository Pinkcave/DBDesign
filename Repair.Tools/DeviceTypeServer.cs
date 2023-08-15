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
    public class DeviceTypeServer
    {
        public static List<Device_Type> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Device_Type.GetName + " where typeid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Device_Type.GetName;
            }
            List<Device_Type> list = new List<Device_Type>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device_Type Object = new Device_Type();
                    Object.TypeID = reader.GetString(0);
                    Object.Type_Name = reader.GetString(1);
                    Object.Brand = reader.GetString(2);
                    Object.Structure_Url = JsonSerializer.Deserialize<List<string>>(reader.GetString(3));
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<Device_Type> QueryByAttribute(string attribute , string value)
        {
            string sql = string.Format("select * from Device_Type where {0} = {1}",attribute, value);
            List<Device_Type> list = new List<Device_Type>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device_Type Object = new Device_Type();
                    Object.TypeID = reader.GetString(0);
                    Object.Type_Name = reader.GetString(1);
                    Object.Brand = reader.GetString(2);
                    Object.Structure_Url = JsonSerializer.Deserialize<List<string>>(reader.GetString(3));
                    list.Add(Object);
                }
            }
            return list;
        }

        public static int Insert(string JsonInfo)
        {
            Device_Type? type = JsonSerializer.Deserialize<Device_Type>(JsonInfo);
            if (type == null)
                return -1;
            string sql = "insert into " + Device_Type.GetName + " values(" + "\'" + type.TypeID + "\'," + "\'" + type.Type_Name + "\'," + "\'" + type.Structure_Url + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Device_Type.GetName + " where typeid=:del_TYPEID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_TYPEID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Device_Type? type = JsonSerializer.Deserialize<Device_Type>(JsonInfo);
            if (type == null)
                return -1;
            string sql = "update " + Device_Type.GetName + " set "
                         + "typeid=:new_TypeID, typename=:new_TypeNAME, StructureUrl=:new_url," + " where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_TypeID", OracleDbType.Varchar2, type.TypeID, ParameterDirection.Input),
                new OracleParameter(":new_TypeNAME", OracleDbType.Varchar2, type.Type_Name, ParameterDirection.Input),
                new OracleParameter(":new_url", OracleDbType.Varchar2, type.Structure_Url, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
