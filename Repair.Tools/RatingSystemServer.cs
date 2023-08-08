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
    public class RatingSystemServer
    {
        public static List<Rating_System> Query(string? uid = null, string? eid=null)
        {
            string sql;
            if (uid != null && eid != null)
            {
                sql = "select * from " + Rating_System.GetName 
                    + " where rateid=" + "\'" + uid + "\'"
                    + " and engineerid=\'" + eid + "\'";
            }
            else if (uid == null && eid != null) 
            {
                sql = "select * from " + Rating_System.GetName
                    + " engineerid=\'" + eid + "\'";
            }
            else if(uid != null && eid == null)
            {
                sql = "select * from " + Rating_System.GetName
                   + " where rateid=" + "\'" + uid + "\'";
            }
            else
            {
                sql = "select * from " + Rating_System.GetName;
            }
            List<Rating_System> ratelist = new List<Rating_System>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Rating_System Object = new Rating_System();
                    Object.UserID = reader.GetString(0);
                    Object.EngineerID = reader.GetString(1);
                    Object.Rate = reader.GetInt32(2);
                    Object.User_Comment = reader.GetString(3);
                    ratelist.Add(Object);
                }
                reader.Close();
            }
            return ratelist;
        }

        public static int Insert(Rating_System rate)
        {
            //Rating_System? rate = JsonSerializer.Deserialize<Rating_System>(JsonInfo);
            if (rate == null)
                return -1;
            string sql = "insert into " + Rating_System.GetName + " values("
                            + "\'" + rate.UserID + "\',"
                            + "\'" + rate.EngineerID + "\',"
                            +        rate.Rate + ","
                            + "\'" + rate.User_Comment + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid, string eid)
        {
            string sql = "delete from " + Rating_System.GetName 
                       + " where rateid=:del_USERID and engineerid=:del_ENGINEERID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_USERID", OracleDbType.Varchar2, uid, ParameterDirection.Input),
                new OracleParameter(":del_ENGINEERID", OracleDbType.Varchar2, eid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_uid, string old_eid)
        {
            Rating_System? rate = JsonSerializer.Deserialize<Rating_System>(JsonInfo);
            if (rate == null)
                return -1;
            string sql = "update " + Rating_System.GetName + " set "
                         + "rateid=:new_USERID, engineerid=:new_ENGINEERID,"
                         + "rate=:new_RATE, rate_comment=:new_USER_COMMENT "
                         + "where rateid=\'" + old_uid + "\' and engineerid=\'" + old_eid + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_USERID", OracleDbType.Varchar2, rate.UserID, ParameterDirection.Input),
                new OracleParameter(":new_ENGINEERID", OracleDbType.Varchar2, rate.EngineerID, ParameterDirection.Input),
                new OracleParameter(":new_RATE", OracleDbType.Int32, rate.Rate, ParameterDirection.Input),
                new OracleParameter(":new_USER_COMMENT", OracleDbType.Varchar2, rate.User_Comment, ParameterDirection.Input)               
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
