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
    public class EngineerServer
    {
        public static List<Engineer> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + Engineer.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + Engineer.GetName;
            }
            List<Engineer> ObjectList = new List<Engineer>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Engineer Object = new Engineer();
                    Object.ID = reader.GetString(0);
                    Object.Name = reader.GetString(1);
                    Object.Sex = reader.GetString(2);
                    Object.WorkingYears = reader.GetInt32(3);
                    Object.Salary = reader.GetFloat(4);
                    Object.Rate = reader.GetInt32(5);
                    Object.Password = reader.GetString(6);
                    Object.WorkingCenter = reader.GetString(7);
                    ObjectList.Add(Object);
                }
                reader.Close();
            }
            return ObjectList;
        }

        public static List<Engineer> QueryByName(string? name=null)
        {
            string sql;
            if(name != null)
            {
                sql = "select * from " + Engineer.GetName + " where name=" + name + "\'";
            }
            else
            {
                sql = "select * from " + Engineer.GetName;
            }
            List<Engineer> ObjectList = new List<Engineer>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    Engineer Object = new Engineer();
                    Object.ID = reader.GetString(0);
                    Object.Name = reader.GetString(1);
                    Object.Sex = reader.GetString(2);
                    Object.WorkingYears = reader.GetInt32(3);
                    Object.Salary = reader.GetFloat(4);
                    Object.Rate = reader.GetInt32(5);
                    Object.Password = reader.GetString(6);
                    Object.WorkingCenter = reader.GetString(7);
                    ObjectList.Add(Object);
                }
                reader.Close();
            }
            return ObjectList;
        }

        public static int Insert(Engineer engineer)
        {
            //Engineer? engineer = JsonSerializer.Deserialize<Engineer>(JsonInfo);
            if (engineer == null)
                return -1;
            string sql = "insert into " + Engineer.GetName + " values("
                            + "\'" + engineer.ID + "\',"
                            + "\'" + engineer.Name + "\',"
                            + "\'" + engineer.Sex + "\',"
                            + engineer.WorkingYears + ","
                            + engineer.Salary + ","
                            + engineer.Rate + ","
                            + "\'" + engineer.Password + "\',"
                            + "\'" + engineer.WorkingCenter + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + Engineer.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(Engineer engineer, string old_id)
        {
            //Engineer? engineer = JsonSerializer.Deserialize<Engineer>(JsonInfo);
            if (engineer == null)
                return -1;
            string sql = "update " + Engineer.GetName + " set "
                         + "id=:new_ID, name=:new_NAME, sex=:new_SEX,"
                         + "workingyears=:new_YEARS, salary=:new_SALARY, rate=:new_RATE,"
                         + "password=:new_PASSWORD, work_center=:new_CENTER "
                         + "where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, engineer.ID, ParameterDirection.Input),
                new OracleParameter(":new_NAME", OracleDbType.Varchar2, engineer.Name, ParameterDirection.Input),
                new OracleParameter(":new_SEX", OracleDbType.Varchar2, engineer.Sex, ParameterDirection.Input),
                new OracleParameter(":new_YEARS", OracleDbType.Int32, engineer.WorkingYears, ParameterDirection.Input),
                new OracleParameter(":new_SALARY", OracleDbType.Varchar2, engineer.Salary, ParameterDirection.Input),
                new OracleParameter(":new_RATE", OracleDbType.Varchar2, engineer.Rate, ParameterDirection.Input),
                new OracleParameter(":new_PASSWORD",OracleDbType.Varchar2, engineer.Password, ParameterDirection.Input),
                new OracleParameter(":new_CENTER",OracleDbType.Varchar2, engineer.WorkingCenter, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
