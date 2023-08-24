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
    public class ServiceLocServer
    {
        public static List<Service_Loc> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Service_Loc.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Service_Loc.GetName;
            }
            List<Service_Loc> list = new List<Service_Loc>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Service_Loc Object = new Service_Loc();
                    Object.ID = reader.GetString(0);
                    Object.Location_Name = reader.GetString(1);
                    Object.Loc_Detail = reader.GetString(2);
                    Object.CustomerID = reader.GetString(3);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<Service_Loc> QueryByAttribute(string attribute,string value)
        {
            string sql = "select * from {0} where {1} = {2}";
            sql = string.Format(sql,Service_Loc.GetName,attribute, value);
            List<Service_Loc> list = new List<Service_Loc>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Service_Loc Object = new Service_Loc();
                    Object.ID = reader.GetString(0);
                    Object.Location_Name = reader.GetString(1);
                    Object.Loc_Detail = reader.GetString(2);
                    Object.CustomerID = reader.GetString(3);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static int Insert(Service_Loc loc)
        {
            //Service_Loc? loc = JsonSerializer.Deserialize<Service_Loc>(JsonInfo);
            if (loc == null)
                return -1;
            string sql = "insert into " + Service_Loc.GetName + " values(" + "\'" + loc.ID + "\'," + "\'" + loc.Location_Name + "\'," + "\'" + loc.Loc_Detail + "\'," + "\'" + loc.CustomerID + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Service_Loc.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(Service_Loc loc, string old_id)
        {
            //Service_Loc? loc = JsonSerializer.Deserialize<Service_Loc>(JsonInfo);
            if (loc == null)
                return -1;
            string sql = "update " + Service_Loc.GetName + " set "
                         + "ID=:new_ID, Location_Name=:new_LocName, Loc_Detail=:new_Detail,"
                         + "CustomerID=:new_CID  where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, loc.ID, ParameterDirection.Input),
                new OracleParameter(":new_LocName", OracleDbType.Varchar2, loc.Location_Name, ParameterDirection.Input),
                new OracleParameter(":new_Loc_Detail", OracleDbType.Varchar2, loc.Loc_Detail, ParameterDirection.Input),
                new OracleParameter(":new_CID", OracleDbType.Varchar2, loc.CustomerID, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Count()
        {
            string sql = "select tablecount from counter where tablename = \'SERVICE_LOC\'";
            int ret = -1;
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                reader.Read();
                ret = reader.GetInt32(0);
            }
            return ret;
        }
    }
}
