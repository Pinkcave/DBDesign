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
    public class DeviceEvalServer
    {
        public static List<Device_Evaluation> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Device_Evaluation.GetName + " where deviceid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Device_Evaluation.GetName;
            }
            List<Device_Evaluation> list = new List<Device_Evaluation>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Device_Evaluation Object = new Device_Evaluation();
                    Object.DeviceID = reader.GetString(0);
                    Object.Evaluation = reader.GetString(1);
                    Object.Price = reader.GetFloat(2);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static int Insert(string JsonInfo)
        {
            Device_Evaluation? eval = JsonSerializer.Deserialize<Device_Evaluation>(JsonInfo);
            if (eval == null)
                return -1;
            string sql = "insert into " + Device_Evaluation.GetName + " values(" + "\'" + eval.DeviceID + "\'," + "\'" + eval.Evaluation + "\'," + "\'" + eval.Price  + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Device_Evaluation.GetName + " where deviceid=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Device_Evaluation? eval = JsonSerializer.Deserialize<Device_Evaluation>(JsonInfo);
            if (eval == null)
                return -1;
            string sql = "update " + Device_Evaluation.GetName + " set "
                         + "deviceid=:new_deviceID, evaluation=:new_eval, price=:new_price,"
                         + "where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_deviceID", OracleDbType.Varchar2, eval.DeviceID, ParameterDirection.Input),
                new OracleParameter(":new_eval", OracleDbType.Varchar2, eval.Evaluation, ParameterDirection.Input),
                new OracleParameter(":new_price", OracleDbType.Varchar2, eval.Price, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
