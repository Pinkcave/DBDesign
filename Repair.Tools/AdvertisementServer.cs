using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;

namespace Repair.Server
{
    public class AdvertisementServer
    {
        public static List<Advertisement> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Advertisement.GetName +" where ID=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Advertisement.GetName;
            }
            List<Advertisement> ObjectList = new List<Advertisement>();
            using(OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Advertisement Object = new Advertisement();
                    Object.ID = reader.GetString(0);
                    Object.Image = reader.GetString(1);
                    Object.Status = reader.GetInt32(2);
                    ObjectList.Add(Object);
                }
                reader.Close();
            }
            return ObjectList;
        }

        public static int Insert(string JsonInfo)
        {
            Advertisement? Object = JsonSerializer.Deserialize<Advertisement>(JsonInfo);
            if (Object == null)
                return -1;
            string sql = "insert into " + Advertisement.GetName + " values(" 
                                        + "\'" + Object.ID + "\'," 
                                        + "\'" + Object.Image + "\'," 
                                        +        Object.Status + ")";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + Advertisement.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Advertisement? Object = JsonSerializer.Deserialize<Advertisement>(JsonInfo);
            if (Object == null)
                return -1;
            string sql = "update " + Advertisement.GetName + " set "
                         + "id=:new_ID, image=:new_IMAGE, status=:new_STATUS "
                         + "where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, Object.ID, ParameterDirection.Input),
                new OracleParameter(":new_IMAGE",OracleDbType.Varchar2, Object.Image, ParameterDirection.Input),
                new OracleParameter(":new_STATUS",OracleDbType.Varchar2, Object.Status, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

    }
}
