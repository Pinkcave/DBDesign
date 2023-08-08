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
    public class CouponServer
    {
        public static List<Coupon> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Coupon.GetName + " where ID=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Coupon.GetName;
            }
            List<Coupon> ObjectList = new List<Coupon>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Coupon Object = new Coupon();
                    Object.Id = reader.GetString(0);
                    Object.Name = reader.GetString(1);
                    Object.Type = reader.GetInt32(2);
                    Object.Discount = reader.GetFloat(3);
                    Object.Status = reader.GetBoolean(4);
                    ObjectList.Add(Object);
                }
                reader.Close();
            }
            return ObjectList;
        }

        public static int Insert(string JsonInfo)
        {
            Coupon? Object = JsonSerializer.Deserialize<Coupon>(JsonInfo);
            if (Object == null)
                return -1;
            string sql = "insert into " + Coupon.GetName + " values("
                         + "\'" + Object.Id + "\',"
                         + "\'" + Object.Name + "\',"
                         +        Object.Type + ","
                         +        Object.Discount + ","
                         +        Object.Status + ")";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }
        public static int Delete(string id)
        {
            string sql = "delete from " + Coupon.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Coupon? Object = JsonSerializer.Deserialize<Coupon>(JsonInfo);
            if (Object == null)
                return -1;
            string sql = "update " + Coupon.GetName + " set "
                         + "id=:new_ID, name=:new_NAME, type=:new_TYPE,"
                         + "discount=:new_DISCOUNT, status=:new_STATUS "
                         + "where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID",OracleDbType.Varchar2,Object.Id, ParameterDirection.Input),
                new OracleParameter(":new_NAME",OracleDbType.Varchar2,Object.Name, ParameterDirection.Input),
                new OracleParameter(":new_TYPE",OracleDbType.Int32,Object.Type, ParameterDirection.Input),
                new OracleParameter(":new_DISCOUNT",OracleDbType.BinaryFloat,Object.Discount, ParameterDirection.Input),
                new OracleParameter(":new_STATUS",OracleDbType.Int32,Object.Status, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
