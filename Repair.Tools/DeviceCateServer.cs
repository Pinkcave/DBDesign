using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;

namespace Repair.Server
{
    public class DeviceCateServer
    {
        public static List<Device_Cate> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Device_Cate.GetName + " where CATEGORYID=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Device_Cate.GetName;
            }
            List<Device_Cate> ObjectList = new List<Device_Cate>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device_Cate Object = new Device_Cate();
                    Object.CategoryID = reader.GetString(0);
                    Object.CategoryName = reader.GetString(1);
                    ObjectList.Add(Object);
                }
                reader.Close();
            }
            return ObjectList;
        }

        public static List<Device_Cate> QueryByAttribute(string attribute,string value)
        {
            string sql = string.Format("select * from Device_Cate where {0} = {1}",attribute, value);
            
            List<Device_Cate> ObjectList = new List<Device_Cate>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device_Cate Object = new Device_Cate();
                    Object.CategoryID = reader.GetString(0);
                    Object.CategoryName = reader.GetString(1);
                    ObjectList.Add(Object);
                }
                reader.Close();
            }
            return ObjectList;
        }

        public static int Insert(string JsonInfo)
        {
            Device_Cate? Object = JsonSerializer.Deserialize<Device_Cate>(JsonInfo);
            if (Object == null)
                return -1;
            string sql = "insert into " + Device_Cate.GetName + " values("
                                        + "\'" + Object.CategoryID + "\',"
                                        + "\'" + Object.CategoryName + "\');";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Device_Cate.GetName + " where categoryid=:del_id";
            OracleParameter[] param =
            {
                new OracleParameter(":del_id",OracleDbType.Varchar2,id,ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo,string id)
        {
            Device_Cate? cate = JsonSerializer.Deserialize<Device_Cate>(JsonInfo);
            if (cate == null)
                return -1;
            string sql = "update " + Device_Cate.GetName + " set "
                         + "CategoryID=:new_CATEID, CategoryName=:new_CATENAME where id=\'" + id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_CATEID", OracleDbType.Varchar2, cate.CategoryID, ParameterDirection.Input),
                new OracleParameter(":new_CATENAME", OracleDbType.Varchar2, cate.CategoryName, ParameterDirection.Input) 
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
