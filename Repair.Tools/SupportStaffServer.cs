using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repair.Server
{
    public class SupportStaffServer
    {
        public static List<Support_Staff> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Support_Staff.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Support_Staff.GetName;
            }
            List<Support_Staff> userlist = new List<Support_Staff>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Support_Staff Object = new Support_Staff();
                    Object.ID = reader.GetString(0);
                    Object.Password = reader.GetString(2);
                    Object.Realname = reader.GetString(3);
                    Object.Nickname = reader.GetString(4);
                    userlist.Add(Object);
                }
                reader.Close();
            }
            return userlist;
        }

        public static int Insert(string JsonInfo)
        {
            Support_Staff? staff = JsonSerializer.Deserialize<Support_Staff>(JsonInfo);
            if (staff == null)
                return -1;
            string sql = "insert into " + Support_Staff.GetName + " values(" + "\'" + staff.ID + "\'," + "\'" + staff.Password + "\'," + "\'" + staff.Realname + "\'," + "\'" + staff.Nickname + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }
        public static int Delete(string uid)
        {
            string sql = "delete from " + Support_Staff.GetName + " where id=:del_USERID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_USERID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Support_Staff? staff = JsonSerializer.Deserialize<Support_Staff>(JsonInfo);
            if (staff == null)
                return -1;
            string sql = "update " + Support_Staff.GetName + " set "
                         + "id=:new_USERID, password=:new_PASSWORD,"
                         + "realname=:new_REALNAME, nickname=:new_NICKNAME"
                         + " where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_USERID", OracleDbType.Varchar2, staff.ID, ParameterDirection.Input),
                new OracleParameter(":new_PASSWORD", OracleDbType.Varchar2, staff.Password, ParameterDirection.Input),
                new OracleParameter(":new_REALNAME", OracleDbType.Varchar2, staff.Realname, ParameterDirection.Input),
                new OracleParameter(":new_NICKNAME", OracleDbType.Varchar2, staff.Nickname, ParameterDirection.Input)

            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
