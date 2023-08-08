﻿using System;
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
    public class RepairOptionServer
    {
        public static List<Repair_Options> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Repair_Options.GetName + " where optionid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Repair_Options.GetName;
            }
            List<Repair_Options> userlist = new List<Repair_Options>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Repair_Options Object = new Repair_Options();
                    Object.OptionID = reader.GetString(0);
                    Object.RepairRequirement = reader.GetString(1);
                    Object.RepairCategoryID = reader.GetString(2);
                    Object.Brand = reader.GetString(3);
                    Object.RepairPrice = reader.GetFloat(4);

                    /*内嵌repair_category对象*/
                    string InnerSql = "select * from REPAIR_CATE where id=\'"
                                    + Object.RepairCategoryID + "\'";
                    using(OracleDataReader InnerReader = DBHelper.GetDataReader(InnerSql, null))
                    {
                        InnerReader.Read();
                        Object.CateName = InnerReader.GetString(1);
                        Object.CateImage = InnerReader.GetString(2);
                        Object.CateDetail = InnerReader.GetString(3);
                    }
                    userlist.Add(Object);
                }
                reader.Close();
            }
            return userlist;
        }

        public static int Insert(string JsonInfo)
        {
            Repair_Options? user = JsonSerializer.Deserialize<Repair_Options>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "insert into " + Repair_Options.GetName + " values("
                            + "\'" + user.OptionID + "\',"
                            + "\'" + user.RepairRequirement + "\',"
                            + "\'" + user.RepairCategoryID + "\',"
                            + "\'" + user.Brand + "\',"
                            + user.RepairPrice + ")";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + Repair_Options.GetName + " where optionid=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Repair_Options? user = JsonSerializer.Deserialize<Repair_Options>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "update " + Repair_Options.GetName + " set "
                         + "optionid=:new_OPTIONID, repairrequirement=:new_REPAIR, repaircategoryid=:new_CATEID,"
                         + "brand=:new_BRAND, repairprice=:new_PRICE "
                         + "where optionid=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_OPTIONID", OracleDbType.Varchar2, user.OptionID, ParameterDirection.Input),
                new OracleParameter(":new_REQUIRE", OracleDbType.Varchar2, user.RepairRequirement, ParameterDirection.Input),
                new OracleParameter(":new_CATEID", OracleDbType.Varchar2, user.RepairCategoryID, ParameterDirection.Input),
                new OracleParameter(":new_BRAND", OracleDbType.Varchar2, user.Brand, ParameterDirection.Input),
                new OracleParameter(":new_PRICE", OracleDbType.BinaryFloat, user.RepairPrice, ParameterDirection.Input)
                
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
