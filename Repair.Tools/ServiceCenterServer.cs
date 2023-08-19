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
    public class ServiceCenterServer
    {
        public static List<Service_Center> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Service_Center.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Service_Center.GetName;
            }
            sql += " order by id";
            List<Service_Center> list = new List<Service_Center>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Service_Center Object = new Service_Center();
                    Object.ID = reader.GetString(0);
                    Object.Name = reader.GetString(1);
                    Object.Loc_Detail = reader.GetString(2);
                    Object.Tel = reader.GetString(3);
                    Object.TIMG_URL = reader.GetString(4);
                    Object.Latitude = reader.GetFloat(5);
                    Object.Longitude = reader.GetFloat(6);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<Service_Center> QueryByAttribute(string attribute, string value)
        {
            string sql;
            sql = "select * from Service_Center where {0} = {1}";
            sql = string.Format(sql, attribute, value);
            List<Service_Center> list = new List<Service_Center>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Service_Center Object = new Service_Center();
                    Object.ID = reader.GetString(0);
                    Object.Name = reader.GetString(1);
                    Object.Loc_Detail = reader.GetString(2);
                    Object.TIMG_URL = reader.GetString(3);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static int Insert(string JsonInfo)
        {
            Service_Center? center = JsonSerializer.Deserialize<Service_Center>(JsonInfo);
            if (center == null)
                return -1;
            string sql = "insert into " + Service_Center.GetName + " values(" + "\'" + center.ID + "\'," + "\'" + center.Name + "\'," + "\'" + center.Loc_Detail + "\'," + center.Tel + ",\'" + center.TIMG_URL + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Service_Center.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Service_Center? center = JsonSerializer.Deserialize<Service_Center>(JsonInfo);
            if (center == null)
                return -1;
            string sql = "update " + Service_Center.GetName + " set "
                         + "id=:new_ID, Name=:new_NAME, Loc_Detail=:new_Loc,"
                         + "Tel=:new_TEL, TIMG_URL=:new_URL where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, center.ID, ParameterDirection.Input),
                new OracleParameter(":new_NAME", OracleDbType.Varchar2, center.Name, ParameterDirection.Input),
                new OracleParameter(":new_Loc", OracleDbType.Varchar2, center.Loc_Detail, ParameterDirection.Input),
                new OracleParameter(":new_TEL", OracleDbType.Int32, center.Tel, ParameterDirection.Input),
                new OracleParameter(":new_URL", OracleDbType.Varchar2, center.TIMG_URL, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
