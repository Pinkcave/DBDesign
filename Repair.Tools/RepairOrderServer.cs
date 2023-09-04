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
    public class RepairOrderServer
    {
        public static List<Repair_Order> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Repair_Order.GetName + " where orderid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Repair_Order.GetName;
            }
            List<Repair_Order> list = new List<Repair_Order>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Repair_Order Object = new Repair_Order();
                    Object.OrderID = reader.GetString(0);
                    Object.OrderPrice = reader.IsDBNull(1) ? null : reader.GetFloat(1);
                    Object.CouponID = reader.IsDBNull(2) ? null : reader.GetString(2);
                    Object.CouObj = CouponServer.Query(Object.CouponID).FirstOrDefault();
                    Object.EngineerID = reader.GetString(3);
                    Object.UserID = reader.GetString(4);
                    Object.RepairOptionID = RepairOptionServer.Query(reader.GetString(5)).FirstOrDefault();
                    Object.CreateTime = reader.GetDateTime(6);
                    Object.RepairLocation = reader.GetString(7);
                    Object.RepairTime = reader.GetDateTime(8);
                    Object.UserRate = reader.IsDBNull(9) ? null : reader.GetString(9);
                    Object.OrderStatus = reader.GetInt32(10);
                    Object.Images = (reader.IsDBNull(11) ? 
                        null : 
                        JsonSerializer.Deserialize<List<string>>(reader.GetString(11)));
                    Object.CustomerLocation = reader.GetString(12);

                    string InnerSql = "select * from COUPON where id=\'" + Object.CouponID + "\'";
                    using (OracleDataReader InnerReader = DBHelper.GetDataReader(InnerSql, null))
                    {
                        InnerReader.Read();
                        Object.CouObj.Name = InnerReader.GetString(1);
                        Object.CouObj.Type = InnerReader.GetInt32(2);
                        Object.CouObj.Discount = InnerReader.GetFloat(3);
                        Object.CouObj.Status = InnerReader.GetBoolean(4);
                    }
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<Repair_Order> QueryByAttribute(string attribute,string value)
        {
            string sql;
            sql = "select * from " + Repair_Order.GetName + " where {0} = {1}";
            sql = string.Format(sql,attribute, value);
            
            List<Repair_Order> list = new List<Repair_Order>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Repair_Order Object = new Repair_Order();
                    Object.OrderID = reader.GetString(0);
                    Object.OrderPrice = reader.IsDBNull(1) ? null : reader.GetFloat(1);
                    Object.CouponID = reader.IsDBNull(2) ? null : reader.GetString(2);
                    Object.CouObj = CouponServer.Query(Object.CouponID).FirstOrDefault();
                    Object.EngineerID = reader.GetString(3);
                    Object.UserID = reader.GetString(4);
                    Object.RepairOptionID = RepairOptionServer.Query(reader.GetString(5)).FirstOrDefault();
                    Object.CreateTime = reader.GetDateTime(6);
                    Object.RepairLocation = reader.GetString(7);
                    Object.RepairTime = reader.GetDateTime(8);
                    Object.UserRate = reader.IsDBNull(9) ? null : reader.GetString(9);
                    Object.OrderStatus = reader.GetInt32(10);
                    Object.Images = reader.IsDBNull(11) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(11));
                    Object.CustomerLocation = reader.GetString(12);

                    string InnerSql = "select * from COUPON where id=\'" + Object.CouponID + "\'";
                    using (OracleDataReader InnerReader = DBHelper.GetDataReader(InnerSql, null))
                    {
                        InnerReader.Read();
                        Object.CouObj.Name = InnerReader.GetString(1);
                        Object.CouObj.Type = InnerReader.GetInt32(2);
                        Object.CouObj.Discount = InnerReader.GetFloat(3);
                        Object.CouObj.Status = InnerReader.GetBoolean(4);
                    }
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static int Insert(Repair_Order user)
        {
            //Repair_Order? user = JsonSerializer.Deserialize<Repair_Order>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "insert into " + Repair_Order.GetName + " values("
                            + "\'" + user.OrderID + "\',"
                            + (user.OrderPrice == null ? "null" : user.OrderPrice) + ","
                            + (user.CouObj == null ? "null," : "\'" + user.CouponID + "\',")
                            + "\'" + user.EngineerID + "\',"
                            + "\'" + user.UserID + "\',"
                            + "\'" + user.RepairOptionID.OptionID + "\',"
                            + "to_date(\'" + user.CreateTime + "\',\'MM/DD/YYYY HH24:MI:SS\'),"
                            //+ "to_date(\'" + user.CreateTime + "\',\'YYYY/MM/DD HH24:MI:SS\'),"
                            + "\'" + user.RepairLocation + "\',"
                            + "to_date(\'" + user.RepairTime + "\',\'MM/DD/YYYY HH24:MI:SS\'),"
                            //+ "to_date(\'" + user.RepairTime + "\',\'YYYY/MM/DD HH24:MI:SS\'),"
                            + (user.UserRate == null ? "null," : "\'" + user.UserRate + "\',")
                            + "\'" + user.OrderStatus + "\'," + FileHelper.UrlListToSet(user.Images) + ","
                            + "\'" + user.CustomerLocation + "\'" + ")";
            Console.WriteLine(sql);
            Console.WriteLine("\n");
            Console.WriteLine(user.RepairTime+" "+user.CreateTime);
            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + Repair_Order.GetName + " where orderid=:del_USERID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_USERID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(Repair_Order? user, string old_id)
        {
            //Repair_Order? user = JsonSerializer.Deserialize<Repair_Order>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "update " + Repair_Order.GetName + " set "
                         + "orderid=:new_ORDERID, orderprice=:new_PRICE, couponid=:new_COUPONID,"
                         + "engineerid=:new_ENGINEERID, userid=:new_USERID, repairoptionid=:new_OPTIONID,"
                         + "createtime=:new_CREATETIME, repairlocation=:new_LOCATION,"
                         + "repairtime=:new_REPAIRTIME, userrate=:new_RATE, orderstatus=:new_STATUS,"
                         + "image_url=" + FileHelper.UrlListToSet(user.Images) + ","
                         + "customer_location=:new_CustomerLoc"
                         + " where orderid=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ORDERID", OracleDbType.Varchar2, user.OrderID, ParameterDirection.Input),
                new OracleParameter(":new_PRICE", OracleDbType.BinaryFloat, user.OrderPrice, ParameterDirection.Input),
                new OracleParameter(":new_COUPONID", OracleDbType.Varchar2, user.CouponID, ParameterDirection.Input),
                new OracleParameter(":new_ENGINEERID", OracleDbType.Varchar2, user.EngineerID, ParameterDirection.Input),
                new OracleParameter(":new_USERID", OracleDbType.Varchar2, user.UserID, ParameterDirection.Input),
                new OracleParameter(":new_OPTIONID", OracleDbType.Varchar2, user.RepairOptionID.OptionID, ParameterDirection.Input),
                new OracleParameter(":new_CREATETIME",OracleDbType.TimeStamp, user.CreateTime, ParameterDirection.Input),
                new OracleParameter(":new_LOCATION",OracleDbType.Varchar2, user.RepairLocation, ParameterDirection.Input),
                new OracleParameter(":new_REPAIRTIME",OracleDbType.TimeStamp, user.RepairTime, ParameterDirection.Input),
                new OracleParameter(":new_RATE",OracleDbType.Varchar2, user.UserRate, ParameterDirection.Input),
                new OracleParameter(":new_STATUS",OracleDbType.Int32, user.OrderStatus, ParameterDirection.Input),
                new OracleParameter(":new_CustomerLoc",OracleDbType.Varchar2, user.CustomerLocation, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Count()
        {
            string sql = "select tablecount from counter where tablename = \'REPAIR_ORDER\'";
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                reader.Read();
                return reader.GetInt32(0);
            }
        }
    }
}
