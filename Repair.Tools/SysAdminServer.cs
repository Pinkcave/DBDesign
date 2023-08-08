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
    public class SysAdminServer
    {

        public static List<Repair_Sys_Admin> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Repair_Sys_Admin.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Repair_Sys_Admin.GetName;
            }
            List<Repair_Sys_Admin> userlist = new List<Repair_Sys_Admin>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Repair_Sys_Admin Object = new Repair_Sys_Admin();
                    Object.ID = reader.GetString(0);
                    Object.Password = reader.GetString(1);
                    Object.Name = reader.GetString(2);
                    userlist.Add(Object);
                }
                reader.Close();
            }
            return userlist;
        }

        public static int Insert(string JsonInfo)
        {
            Repair_Sys_Admin? admin = JsonSerializer.Deserialize<Repair_Sys_Admin>(JsonInfo);
            if (admin == null)
                return -1;
            string sql = "insert into " + Repair_Sys_Admin.GetName + " values(" + "\'" + admin.ID + "\'," + "\'" + admin.Password + "\'," + "\'" + admin.Name + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string ID)
        {
            string sql = "delete from " + Repair_Sys_Admin.GetName + " where id=:del_ADMINID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ADMINID", OracleDbType.Varchar2, ID, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Repair_Sys_Admin? user = JsonSerializer.Deserialize<Repair_Sys_Admin>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "update " + Repair_Sys_Admin.GetName + " set "
                         + "ID=:new_ADMINID, Password=:new_PASSWORD, Name=:new_ADMINNAME "
                         + " where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ADMINID", OracleDbType.Varchar2, user.ID, ParameterDirection.Input),
                new OracleParameter(":new_PASSWORD", OracleDbType.Varchar2, user.Password, ParameterDirection.Input),
                new OracleParameter(":new_ADMINNAME", OracleDbType.Varchar2, user.Name, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
