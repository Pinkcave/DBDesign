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
    public class RepairCateServer
    {
        public static List<Repair_Cate> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Repair_Cate.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Repair_Cate.GetName;
            }
            List<Repair_Cate> userlist = new List<Repair_Cate>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Repair_Cate Object = new Repair_Cate();
                    Object.ID = reader.GetString(0);
                    Object.Name = reader.GetString(1);
                    Object.Image = reader.GetString(2);
                    Object.Detail = reader.GetString(3);
                    userlist.Add(Object);
                }
                reader.Close();
            }
            return userlist;
        }

        public static int Insert(string JsonInfo)
        {
            Repair_Cate? user = JsonSerializer.Deserialize<Repair_Cate>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "insert into " + Repair_Cate.GetName + " values("
                            + "\'" + user.ID + "\',"
                            + "\'" + user.Name + "\',"
                            + "\'" + user.Image + "\',"
                            + "\'" + user.Detail + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + Repair_Cate.GetName + " where id=:del_USERID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_USERID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(string JsonInfo, string old_id)
        {
            Repair_Cate? user = JsonSerializer.Deserialize<Repair_Cate>(JsonInfo);
            if (user == null)
                return -1;
            string sql = "update " + Repair_Cate.GetName + " set "
                         + "id=:new_ID, name=:new_NAME,"
                         + "image=:new_IMAGE, detail=:new_DETAIL "
                         + "where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, user.ID, ParameterDirection.Input),
                new OracleParameter(":new_NAME", OracleDbType.Varchar2, user.Name, ParameterDirection.Input),
                new OracleParameter(":new_IMAGE", OracleDbType.Varchar2, user.Image, ParameterDirection.Input),
                new OracleParameter(":new_DETAIL",OracleDbType.Varchar2, user.Detail, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
