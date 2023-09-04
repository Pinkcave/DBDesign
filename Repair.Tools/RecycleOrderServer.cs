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
    public class RecycleOrderServer
    {
        public static List<Recycle_Order> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Recycle_Order.GetName + " where orderid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Recycle_Order.GetName;
            }
            List<Recycle_Order> list = new List<Recycle_Order>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Recycle_Order Object = new Recycle_Order();
                    Object.OrderID = reader.GetString(0);
                    Device device = DeviceServer.Query(reader.GetString(1)).FirstOrDefault();
                    Object.Device = device;
                    Object.UserID = reader.GetString(2);
                    Object.ExpectedPrice = reader.GetFloat(3);
                    Object.Recycle_Location = reader.GetString(4);
                    Object.Recycle_Time = reader.GetDateTime(5);
                    Object.Images = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(6));
                    Object.CustomerLocation = reader.GetString(7);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<Recycle_Order> QueryByAttribute(string attribute,string value)
        {
            string sql;
            sql = "select * from Recycle_Order where {0} = {1}";
            sql = string.Format(sql,attribute, value);
            List<Recycle_Order> list = new List<Recycle_Order>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Recycle_Order Object = new Recycle_Order();
                    Object.OrderID = reader.GetString(0);
                    Device device = DeviceServer.Query(reader.GetString(1)).FirstOrDefault();
                    Object.Device = device;
                    Object.UserID = reader.GetString(2);
                    Object.ExpectedPrice = reader.GetFloat(3);
                    Object.Recycle_Location = reader.GetString(4);
                    Object.Recycle_Time = reader.GetDateTime(5);
                    Object.Images = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(6));
                    Object.CustomerLocation = reader.GetString(7);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static int Insert(Recycle_Order order)
        {
            //Recycle_Order? order = JsonSerializer.Deserialize<Recycle_Order>(JsonInfo);
            if (order == null)
                return -1;
            //string sql = "insert into " + Recycle_Order.GetName + " values(" + "\'" + order.OrderID + "\'," + "\'" + order.Device.DeviceID + "\'," + "\'" + order.UserID + "\'," + order.ExpectedPrice + ",\'" + order.Recycle_Location + "\',"
             //   + order.Recycle_Time + ")";
            string sql = string.Format("insert into {0} values(\'{1}\',\'{2}\',\'{3}\',{4},\'{5}\',to_date(\'{6}\',\'MM/DD/YYYY HH24:MI:SS\'),{7},\'{8}\')", Recycle_Order.GetName, order.OrderID, order.Device.DeviceID, order.UserID, order.ExpectedPrice, order.Recycle_Location, order.Recycle_Time,FileHelper.UrlListToSet(order.Images), order.CustomerLocation);
            //string sql = string.Format("insert into {0} values(\'{1}\',\'{2}\',\'{3}\',{4},\'{5}\',to_date(\'{6}\',\'YYYY/MM/DD HH24:MI:SS\'),{7},\'{8}\')", Recycle_Order.GetName, order.OrderID, order.Device.DeviceID, order.UserID, order.ExpectedPrice, order.Recycle_Location, order.Recycle_Time, FileHelper.UrlListToSet(order.Images), order.CustomerLocation);
            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + Recycle_Order.GetName + " where orderid=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(Recycle_Order order, string old_id)
        {
            //Recycle_Order? order = JsonSerializer.Deserialize<Recycle_Order>(JsonInfo);
            if (order == null)
                return -1;
            string sql = "update " + Recycle_Order.GetName + " set "
                         + "OrderID=:new_OrderID, DeviceID=:new_DeviceID, UserID=:new_UserID,"
                         + "ExpectedPrice=:new_Price, Recycle_Location=:new_Loc, Recycle_Time=:new_Time, "
                         + "image_url=" + FileHelper.UrlListToSet(order.Images) + ", "
                         + "customer_location=:new_Customer_Loc"
                         + " where orderid=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_OrderID", OracleDbType.Varchar2, order.OrderID, ParameterDirection.Input),
                new OracleParameter(":new_DeviceID", OracleDbType.Varchar2, order.Device.DeviceID, ParameterDirection.Input),
                new OracleParameter(":new_UserID", OracleDbType.Varchar2, order.UserID, ParameterDirection.Input),
                new OracleParameter(":new_Price", OracleDbType.BinaryFloat, order.ExpectedPrice, ParameterDirection.Input),
                new OracleParameter(":new_Loc", OracleDbType.Varchar2, order.Recycle_Location, ParameterDirection.Input),
                new OracleParameter(":new_Time", OracleDbType.TimeStamp, order.Recycle_Time, ParameterDirection.Input),
                new OracleParameter(":new_Customer_Loc", OracleDbType.Varchar2, order.CustomerLocation, ParameterDirection.Input),
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Count()
        {
            string sql = "select tablecount from counter where tablename = \'RECYCLE_ORDER\'";
            using (OracleDataReader reader = DBHelper.GetDataReader(sql,null))
            {
                reader.Read();
                return reader.GetInt32(0);
            }
        }
    }
}
