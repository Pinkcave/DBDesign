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
    public class UserServer
    {
        private static int usernum = 10000;
        public static int UserNum
        {
            get { return usernum; }
            set { usernum = value; }
        }
        

        public static List<UserInfo> Query(string? id = null)
        {
            string sql;
            if(id != null)
            {
                sql = "select * from " + UserInfo.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + UserInfo.GetName;
            }
            List<UserInfo> userlist = new List<UserInfo>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    UserInfo Object = new UserInfo();
                    Object.UserId = reader.GetString(0);
                    Object.UserName = reader.GetString(1);
                    Object.Password = reader.GetString(2);
                    Object.Level = reader.GetInt32(3);
                    Object.Name = reader.GetString(4);
                    Object.Identity = reader.GetString(5);
                    Object.Telephone = reader.GetString(6);
                    Object.Email = reader.GetString(7);
                    Object.Balance = reader.GetFloat(8);
                    userlist.Add(Object);
                }
                reader.Close();
            }
            return userlist;
        }

        public static int Insert(UserInfo user)
        {
            //UserInfo? user = JsonSerializer.Deserialize<UserInfo>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "insert into " + UserInfo.GetName + " values(" + "\'" + user.UserId + "\'," + "\'" + user.UserName + "\'," + "\'" + user.Password + "\'," + user.Level + ",\'" + user.Name + "\',"
                + "\'" + user.Identity + "\'," + "\'" + user.Telephone + "\'," + "\'" + user.Email + "\'," + user.Balance + ")";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + UserInfo.GetName + " where id=:del_USERID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_USERID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(UserInfo user, string old_id)
        {
            //UserInfo? user = JsonSerializer.Deserialize<UserInfo>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "update " + UserInfo.GetName + " set "
                         + "id=:new_USERID, username=:new_USERNAME, password=:new_PASSWORD,"
                         + "userlevel=:new_LEVEL, name=:new_NAME, identify=:new_IDENTITY,"
                         + "telephone=:new_TELEPHONE, email=:new_EMAIL, balance=:new_BALANCE where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_USERID", OracleDbType.Varchar2, user.UserId, ParameterDirection.Input),
                new OracleParameter(":new_USERNAME", OracleDbType.Varchar2, user.UserName, ParameterDirection.Input),
                new OracleParameter(":new_PASSWORD", OracleDbType.Varchar2, user.Password, ParameterDirection.Input),
                new OracleParameter(":new_LEVEL", OracleDbType.Int32, user.Level, ParameterDirection.Input),
                new OracleParameter(":new_NAME", OracleDbType.Varchar2, user.Name, ParameterDirection.Input),
                new OracleParameter(":new_IDENTITY", OracleDbType.Varchar2, user.Identity, ParameterDirection.Input),
                new OracleParameter(":new_TELEPHONE",OracleDbType.Varchar2, user.Telephone, ParameterDirection.Input),
                new OracleParameter(":new_EMAIL",OracleDbType.Varchar2, user.Email, ParameterDirection.Input),
                new OracleParameter(":new_BALANCE",OracleDbType.Double, user.Balance, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
        /*
        public int BalanceManage(string uid,int newbalance)
        {
            string sql = string.Format("update userinfo set balance = {0} where id = {1}", newbalance, uid);
            return DBHelper.RunExecNonQuery(sql, null);
        }*/

        public static int Count()
        {
            string sql = "select tablecount from counter where tablename = \'USERINFO\'";
            int ret = -1;
            using (OracleDataReader reader = DBHelper.GetDataReader(sql,null))
            {
                reader.Read();
                ret = reader.GetInt32(0);
            }
            return ret;
        }
    }
}